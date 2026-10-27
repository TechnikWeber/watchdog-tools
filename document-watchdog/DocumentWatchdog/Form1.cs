using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DocumentWatchdog
{
    public partial class Form1 : Form
    {
        // ── State ──────────────────────────────────────────────────────────────
        private System.Windows.Forms.Timer watchTimer = new();
        private System.Windows.Forms.Timer blinkTimer = new();
        private string watchedFilePath = string.Empty;
        private long lastFileSize = -1;
        private long currentFileSize = -1;
        private bool alertActive = false;
        private int blinkCounter = 0;
        private bool isWatching = false;

        // ── WinAPI for flash/foreground ────────────────────────────────────────
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public Form1()
        {
            InitializeComponent();
            SetupTimers();
            PopulateIntervalCombo();
            UpdateUI();
        }

        // ── Timer Setup ───────────────────────────────────────────────────────
        private void SetupTimers()
        {
            watchTimer.Tick += WatchTimer_Tick;

            blinkTimer.Interval = 400;
            blinkTimer.Tick += BlinkTimer_Tick;
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

        // ── Button Events ─────────────────────────────────────────────────────
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Zu überwachende Datei auswählen",
                Filter = "Alle Dateien (*.*)|*.*"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                watchedFilePath = dlg.FileName;
                txtFilePath.Text = watchedFilePath;
                lblFileSize.Text = "Dateigröße: –";
                lastFileSize = -1;
                UpdateUI();
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (isWatching)
                StopWatching();
            else
                StartWatching();
        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            DismissAlert();
        }

        private void btnTestEmail_Click(object sender, EventArgs e)
        {
            if (!ValidateEmailSettings()) return;
            try
            {
                SendEmail("DocumentWatchdog – Testmail",
                    "Dies ist eine Testmail vom DocumentWatchdog.\nDie E-Mail-Einstellungen funktionieren korrekt.");
                MessageBox.Show("Testmail erfolgreich gesendet!", "Erfolg",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Senden:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Watch Logic ───────────────────────────────────────────────────────
        private void StartWatching()
        {
            if (string.IsNullOrWhiteSpace(watchedFilePath) || !File.Exists(watchedFilePath))
            {
                MessageBox.Show("Bitte wählen Sie zuerst eine gültige Datei aus.",
                    "Keine Datei", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbInterval.SelectedItem is not IntervalItem item) return;

            lastFileSize = GetFileSize();
            currentFileSize = lastFileSize;
            lblFileSize.Text = $"Dateigröße: {FormatBytes(lastFileSize)}";
            lblStatus.Text = "Status: Überwachung läuft …";
            lblStatus.ForeColor = Color.FromArgb(0, 200, 100);

            watchTimer.Interval = item.Milliseconds;
            watchTimer.Start();
            isWatching = true;
            DismissAlert();
            UpdateUI();
            LogEvent($"Überwachung gestartet – Intervall: {item}, Datei: {watchedFilePath}");
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
            if (!File.Exists(watchedFilePath))
            {
                LogEvent("⚠ Datei nicht mehr gefunden!");
                TriggerAlert("Datei nicht gefunden", $"Die Datei\n{watchedFilePath}\nwurde nicht gefunden.");
                StopWatching();
                return;
            }

            currentFileSize = GetFileSize();
            lblFileSize.Text = $"Dateigröße: {FormatBytes(currentFileSize)}  (vorher: {FormatBytes(lastFileSize)})";

            if (currentFileSize <= lastFileSize && !alertActive)
            {
                string msg = currentFileSize < lastFileSize
                    ? $"Die Dateigröße ist gesunken!\n\nVorher: {FormatBytes(lastFileSize)}\nJetzt:   {FormatBytes(currentFileSize)}"
                    : $"Die Dateigröße ist NICHT gestiegen!\n\nGröße: {FormatBytes(currentFileSize)}";

                LogEvent($"⚠ ALARM: {msg.Replace("\n", " ")}");
                TriggerAlert("Kein Dateiwachstum erkannt!", msg);
            }
            else if (currentFileSize > lastFileSize)
            {
                LogEvent($"✔ Größe gestiegen: {FormatBytes(lastFileSize)} → {FormatBytes(currentFileSize)}");
                lastFileSize = currentFileSize;
            }
        }

        // ── Alert ─────────────────────────────────────────────────────────────
        private void TriggerAlert(string title, string message)
        {
            alertActive = true;
            blinkCounter = 0;
            blinkTimer.Start();

            // Alarm sound (Windows built-in)
            if (chkSound.Checked)
            {
                PlayAlarmSound();
            }

            // Popup
            ShowAlertPopup(title, message);

            // Email
            if (chkEmail.Checked)
            {
                try
                {
                    SendEmail($"DocumentWatchdog ALARM: {title}",
                        $"{message}\n\nDatei: {watchedFilePath}\nZeitpunkt: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
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

            // Nach 30 Blinks (ca. 12 Sek.) nur noch langsam blinken
            if (blinkCounter > 30) blinkTimer.Interval = 1200;
        }

        // ── Sound ─────────────────────────────────────────────────────────────
        private static void PlayAlarmSound()
        {
            // Windows Exclamation sound, mehrfach wiederholt
            for (int i = 0; i < 4; i++)
            {
                SystemSounds.Exclamation.Play();
                System.Threading.Thread.Sleep(300);
            }
            // Zusätzlich Windows Beeps als Alarmton
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
        private long GetFileSize()
        {
            try { return new FileInfo(watchedFilePath).Length; }
            catch { return -1; }
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes < 0) return "unbekannt";
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1048576) return $"{bytes / 1024.0:F2} KB";
            if (bytes < 1073741824) return $"{bytes / 1048576.0:F2} MB";
            return $"{bytes / 1073741824.0:F2} GB";
        }

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
            bool hasFile = !string.IsNullOrWhiteSpace(watchedFilePath);
            btnStartStop.Text = isWatching ? "⏹ Stoppen" : "▶ Starten";
            btnStartStop.BackColor = isWatching
                ? Color.FromArgb(160, 40, 40)
                : Color.FromArgb(30, 140, 80);
            btnDismiss.Visible = alertActive;
            cmbInterval.Enabled = !isWatching;
            btnBrowse.Enabled = !isWatching;
            btnStartStop.Enabled = hasFile || isWatching;

            // Email-Panel
            panelEmailSettings.Visible = chkEmail.Checked;
        }

        private void chkEmail_CheckedChanged(object sender, EventArgs e) => UpdateUI();

        // ── Helper class ──────────────────────────────────────────────────────
        private sealed record IntervalItem(string Label, int Milliseconds)
        {
            public override string ToString() => Label;
        }
    }
}