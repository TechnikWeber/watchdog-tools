using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProgramWatchdog
{
    public partial class Form1 : Form
    {
        // ── State ──────────────────────────────────────────────────────────────
        private System.Windows.Forms.Timer watchTimer = new();
        private System.Windows.Forms.Timer blinkTimer = new();
        private System.Windows.Forms.Timer refreshProcessTimer = new();

        private int watchedProcessId = -1;
        private string watchedProcessName = string.Empty;
        private string watchedProcessTitle = string.Empty;

        private bool alertActive = false;
        private bool isWatching = false;
        private int blinkCounter = 0;

        // ── WinAPI ────────────────────────────────────────────────────────────
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        [DllImport("user32.dll")]
        private static extern bool IsHungAppWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessageTimeout(
            IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam,
            uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

        private const uint WM_NULL = 0x0000;
        private const uint SMTO_ABORTIFHUNG = 0x0002;

        public Form1()
        {
            InitializeComponent();
            SetupTimers();
            PopulateIntervalCombo();
            RefreshProcessList();
            UpdateUI();
        }

        // ── Timer Setup ───────────────────────────────────────────────────────
        private void SetupTimers()
        {
            watchTimer.Tick += WatchTimer_Tick;

            blinkTimer.Interval = 400;
            blinkTimer.Tick += BlinkTimer_Tick;

            refreshProcessTimer.Interval = 3000;
            refreshProcessTimer.Tick += (s, e) =>
            {
                if (!isWatching) RefreshProcessList();
            };
            refreshProcessTimer.Start();
        }

        private void PopulateIntervalCombo()
        {
            cmbInterval.Items.Clear();
            cmbInterval.Items.Add(new IntervalItem("1 Minute", 60_000));
            cmbInterval.Items.Add(new IntervalItem("5 Minuten", 300_000));
            cmbInterval.Items.Add(new IntervalItem("10 Minuten", 600_000));
            cmbInterval.Items.Add(new IntervalItem("15 Minuten", 900_000));
            cmbInterval.Items.Add(new IntervalItem("30 Minuten", 1_800_000));
            cmbInterval.Items.Add(new IntervalItem("1 Stunde", 3_600_000));
            cmbInterval.Items.Add(new IntervalItem("6 Stunden", 21_600_000));
            cmbInterval.Items.Add(new IntervalItem("8 Stunden", 28_800_000));
            cmbInterval.Items.Add(new IntervalItem("12 Stunden", 43_200_000));
            cmbInterval.Items.Add(new IntervalItem("24 Stunden", 86_400_000));
            cmbInterval.SelectedIndex = 0;
        }

        // ── Process List ──────────────────────────────────────────────────────
        private void RefreshProcessList()
        {
            try
            {
                var selected = lstProcesses.SelectedItem as ProcessItem;

                var processes = Process.GetProcesses()
                    .Where(p =>
                    {
                        try { return p.MainWindowHandle != IntPtr.Zero && !string.IsNullOrEmpty(p.MainWindowTitle); }
                        catch { return false; }
                    })
                    .OrderBy(p => p.ProcessName)
                    .Select(p => new ProcessItem(p))
                    .ToList();

                lstProcesses.BeginUpdate();
                lstProcesses.Items.Clear();
                foreach (var item in processes)
                    lstProcesses.Items.Add(item);

                // Re-select previously selected process
                if (selected != null)
                {
                    var reselect = lstProcesses.Items.Cast<ProcessItem>()
                        .FirstOrDefault(x => x.ProcessId == selected.ProcessId);
                    if (reselect != null)
                        lstProcesses.SelectedItem = reselect;
                }

                lstProcesses.EndUpdate();
                lblProcessCount.Text = $"{processes.Count} laufende Prozesse mit Fenster";
            }
            catch { }
        }

        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem is ProcessItem item)
            {
                watchedProcessId = item.ProcessId;
                watchedProcessName = item.ProcessName;
                watchedProcessTitle = item.WindowTitle;
                lblSelectedProcess.Text = $"Ausgewählt: {item.ProcessName} (PID {item.ProcessId})";
                lblSelectedProcess.ForeColor = Color.FromArgb(80, 220, 130);
            }
            UpdateUI();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
            LogEvent("Prozessliste aktualisiert.");
        }

        // ── Watch Logic ───────────────────────────────────────────────────────
        private void StartWatching()
        {
            if (watchedProcessId < 0)
            {
                MessageBox.Show("Bitte wählen Sie zuerst einen Prozess aus.",
                    "Kein Prozess", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verify process still exists
            try { Process.GetProcessById(watchedProcessId); }
            catch
            {
                MessageBox.Show("Der gewählte Prozess existiert nicht mehr.\nBitte Liste aktualisieren und neu auswählen.",
                    "Prozess nicht gefunden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RefreshProcessList();
                return;
            }

            if (cmbInterval.SelectedItem is not IntervalItem item) return;

            watchTimer.Interval = item.Milliseconds;
            watchTimer.Start();
            isWatching = true;
            DismissAlert();
            UpdateUI();
            lblStatus.Text = "Status: Überwachung läuft …";
            lblStatus.ForeColor = Color.FromArgb(0, 200, 100);
            LogEvent($"Überwachung gestartet – Intervall: {item}, Prozess: {watchedProcessName} (PID {watchedProcessId})");
        }

        private void StopWatching()
        {
            watchTimer.Stop();
            isWatching = false;
            lblStatus.Text = "Status: Gestoppt";
            lblStatus.ForeColor = Color.FromArgb(180, 180, 180);
            UpdateUI();
            LogEvent("Überwachung gestoppt.");
        }

        private void WatchTimer_Tick(object? sender, EventArgs e)
        {
            Process? proc = null;
            try { proc = Process.GetProcessById(watchedProcessId); }
            catch
            {
                LogEvent($"⚠ Prozess '{watchedProcessName}' (PID {watchedProcessId}) nicht mehr gefunden – beendet oder abgestürzt?");
                TriggerAlert("Prozess nicht mehr vorhanden!",
                    $"Der Prozess\n{watchedProcessName} (PID {watchedProcessId})\nwurde nicht mehr gefunden.\n\nMöglicherweise abgestürzt oder beendet.");
                StopWatching();
                return;
            }

            // Check 1: Windows API IsHungAppWindow
            if (proc.MainWindowHandle != IntPtr.Zero && IsHungAppWindow(proc.MainWindowHandle))
            {
                LogEvent($"⚠ ALARM: IsHungAppWindow meldet Einfrieren für '{watchedProcessName}'");
                TriggerAlert("Programm reagiert nicht!",
                    $"Das Programm\n{watchedProcessName} (PID {watchedProcessId})\nreagiert nicht mehr (Windows meldet 'Not Responding').");
                return;
            }

            // Check 2: SendMessageTimeout – sendet WM_NULL ans Hauptfenster
            if (proc.MainWindowHandle != IntPtr.Zero)
            {
                int result = SendMessageTimeout(
                    proc.MainWindowHandle, WM_NULL, IntPtr.Zero, IntPtr.Zero,
                    SMTO_ABORTIFHUNG, 5000, out _);

                if (result == 0 && !alertActive)
                {
                    LogEvent($"⚠ ALARM: SendMessageTimeout Timeout für '{watchedProcessName}' – Einfrieren erkannt!");
                    TriggerAlert("Programm eingefroren!",
                        $"Das Programm\n{watchedProcessName} (PID {watchedProcessId})\nhat nicht innerhalb von 5 Sekunden auf eine Nachricht reagiert.\n\nMögliche Ursache: Einfrieren / Deadlock.");
                    return;
                }
            }

            // Check 3: Responding-Property
            if (!proc.Responding && !alertActive)
            {
                LogEvent($"⚠ ALARM: Process.Responding = false für '{watchedProcessName}'");
                TriggerAlert("Programm reagiert nicht!",
                    $"Das Programm\n{watchedProcessName} (PID {watchedProcessId})\nhat den Status 'Reagiert nicht'.");
                return;
            }

            if (!alertActive)
            {
                LogEvent($"✔ '{watchedProcessName}' läuft normal (Responding: {proc.Responding})");
            }
        }

        // ── Alert ─────────────────────────────────────────────────────────────
        private void TriggerAlert(string title, string message)
        {
            if (alertActive) return;
            alertActive = true;
            blinkCounter = 0;
            blinkTimer.Interval = 400;
            blinkTimer.Start();

            if (chkSound.Checked) PlayAlarmSound();

            ShowAlertPopup(title, message);

            if (chkEmail.Checked)
            {
                try
                {
                    SendEmail($"ProgramWatchdog ALARM: {title}",
                        $"{message}\n\nProzess: {watchedProcessName} (PID {watchedProcessId})\nZeitpunkt: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    LogEvent("E-Mail-Benachrichtigung gesendet.");
                }
                catch (Exception ex)
                {
                    LogEvent($"E-Mail-Fehler: {ex.Message}");
                }
            }

            lblStatus.Text = "Status: ⚠ ALARM!";
            lblStatus.ForeColor = Color.Red;
            UpdateUI();
        }

        private void DismissAlert()
        {
            alertActive = false;
            blinkTimer.Stop();
            panelAlert.Visible = false;
            this.BackColor = Color.FromArgb(30, 30, 40);
            if (isWatching)
            {
                lblStatus.Text = "Status: Überwachung läuft …";
                lblStatus.ForeColor = Color.FromArgb(0, 200, 100);
            }
            UpdateUI();
        }

        private void ShowAlertPopup(string title, string message)
        {
            panelAlert.Visible = true;
            lblAlertTitle.Text = $"🚨 {title}";
            lblAlertMessage.Text = message;
            SetForegroundWindow(this.Handle);
            this.BringToFront();
            this.Activate();
        }

        private void BlinkTimer_Tick(object? sender, EventArgs e)
        {
            blinkCounter++;
            bool isRed = (blinkCounter % 2 == 0);
            this.BackColor = isRed ? Color.FromArgb(160, 0, 0) : Color.FromArgb(30, 30, 40);
            panelAlert.BackColor = isRed ? Color.FromArgb(200, 0, 0) : Color.FromArgb(120, 0, 0);
            FlashWindow(this.Handle, true);
            if (blinkCounter > 30) blinkTimer.Interval = 1200;
        }

        // ── Button Events ─────────────────────────────────────────────────────
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (isWatching) StopWatching();
            else StartWatching();
        }

        private void btnDismiss_Click(object sender, EventArgs e) => DismissAlert();

        private void btnTestEmail_Click(object sender, EventArgs e)
        {
            if (!ValidateEmailSettings()) return;
            try
            {
                SendEmail("ProgramWatchdog – Testmail",
                    "Dies ist eine Testmail vom ProgramWatchdog.\nDie E-Mail-Einstellungen funktionieren korrekt.");
                MessageBox.Show("Testmail erfolgreich gesendet!", "Erfolg",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Senden:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Sound ─────────────────────────────────────────────────────────────
        private static void PlayAlarmSound()
        {
            for (int i = 0; i < 4; i++)
            {
                SystemSounds.Exclamation.Play();
                System.Threading.Thread.Sleep(300);
            }
            System.Threading.Tasks.Task.Run(() =>
            {
                for (int i = 0; i < 6; i++)
                {
                    Console.Beep(880, 200);
                    Console.Beep(660, 200);
                }
            });
        }

        // ── Email ─────────────────────────────────────────────────────────────
        private bool ValidateEmailSettings()
        {
            if (string.IsNullOrWhiteSpace(txtSmtpHost.Text) ||
                string.IsNullOrWhiteSpace(txtSmtpUser.Text) ||
                string.IsNullOrWhiteSpace(txtSmtpPass.Text) ||
                string.IsNullOrWhiteSpace(txtEmailTo.Text))
            {
                MessageBox.Show("Bitte alle E-Mail-Felder ausfüllen.", "Fehlende Eingaben",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void SendEmail(string subject, string body)
        {
            using var client = new SmtpClient(txtSmtpHost.Text, (int)nudSmtpPort.Value)
            {
                EnableSsl = chkSmtpSsl.Checked,
                Credentials = new NetworkCredential(txtSmtpUser.Text, txtSmtpPass.Text),
                Timeout = 15_000
            };
            using var mail = new MailMessage(txtSmtpUser.Text, txtEmailTo.Text, subject, body);
            client.Send(mail);
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private void LogEvent(string text)
        {
            string line = $"[{DateTime.Now:HH:mm:ss}] {text}";
            if (lstLog.InvokeRequired)
                lstLog.Invoke(() => { lstLog.Items.Insert(0, line); });
            else
                lstLog.Items.Insert(0, line);
            if (lstLog.Items.Count > 200) lstLog.Items.RemoveAt(lstLog.Items.Count - 1);
        }

        private void UpdateUI()
        {
            btnStartStop.Text = isWatching ? "⏹ Stoppen" : "▶ Starten";
            btnStartStop.BackColor = isWatching
                ? Color.FromArgb(160, 40, 40)
                : Color.FromArgb(30, 140, 80);
            btnDismiss.Visible = alertActive;
            cmbInterval.Enabled = !isWatching;
            lstProcesses.Enabled = !isWatching;
            btnRefresh.Enabled = !isWatching;
            btnStartStop.Enabled = watchedProcessId >= 0 || isWatching;
            panelEmailSettings.Visible = chkEmail.Checked;
        }

        private void chkEmail_CheckedChanged(object sender, EventArgs e) => UpdateUI();

        // ── Helper classes ────────────────────────────────────────────────────
        private sealed class ProcessItem
        {
            public int ProcessId { get; }
            public string ProcessName { get; }
            public string WindowTitle { get; }

            public ProcessItem(Process p)
            {
                ProcessId = p.Id;
                ProcessName = p.ProcessName;
                try { WindowTitle = p.MainWindowTitle; } catch { WindowTitle = ""; }
            }

            public override string ToString()
            {
                string title = WindowTitle.Length > 50
                    ? WindowTitle[..47] + "…"
                    : WindowTitle;
                return $"{ProcessName,-30} PID {ProcessId,6}   {title}";
            }
        }

        private sealed record IntervalItem(string Label, int Milliseconds)
        {
            public override string ToString() => Label;
        }
    }
}