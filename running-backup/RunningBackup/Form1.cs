using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RunningBackup
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer _backupTimer = null!;
        private System.Windows.Forms.Timer _progressTimer = null!;
        private string _sourceFile = "";
        private string _sourceFolder = "";
        private string _outputDirectory = "";
        private string _outputBaseName = "";
        private DateTime _lastBackupTime;
        private int _intervalMs;
        private bool _folderMode = false;

        private readonly Dictionary<string, int> _intervals = new()
        {
            { "1 Minute",   1 },
            { "5 Minuten",  5 },
            { "10 Minuten", 10 },
            { "15 Minuten", 15 },
            { "30 Minuten", 30 },
            { "1 Stunde",   60 },
            { "6 Stunden",  360 },
            { "12 Stunden", 720 },
            { "24 Stunden", 1440 },
        };

        public Form1()
        {
            InitializeComponent();
            InitializeTimers();
            InitializeIntervalDropdown();
            UpdateModeUI();
        }

        private void InitializeTimers()
        {
            _backupTimer = new System.Windows.Forms.Timer();
            _backupTimer.Tick += BackupTimer_Tick;

            _progressTimer = new System.Windows.Forms.Timer();
            _progressTimer.Interval = 1000;
            _progressTimer.Tick += ProgressTimer_Tick;
        }

        private void InitializeIntervalDropdown()
        {
            foreach (var key in _intervals.Keys)
                cmbInterval.Items.Add(key);
            cmbInterval.SelectedIndex = 5;
        }

        private int GetSelectedIntervalMs()
        {
            if (cmbInterval.SelectedItem is string selected && _intervals.TryGetValue(selected, out int minutes))
                return minutes * 60 * 1000;
            return 60 * 60 * 1000;
        }

        private void rbFile_CheckedChanged(object? sender, EventArgs e)
        {
            _folderMode = rbFolder.Checked;
            UpdateModeUI();
        }

        private void UpdateModeUI()
        {
            if (_folderMode)
            {
                lblSource.Text = "Quellordner:";
                btnSelectSource.Text = "Ordner...";
                lblBaseName.Visible = false;
                txtBaseName.Visible = false;
                txtSourceFile.Text = _sourceFolder;
            }
            else
            {
                lblSource.Text = "Quelldatei:";
                btnSelectSource.Text = "Datei...";
                lblBaseName.Visible = true;
                txtBaseName.Visible = true;
                txtSourceFile.Text = _sourceFile;
            }
        }

        private void btnSelectSource_Click(object? sender, EventArgs e)
        {
            if (_folderMode)
            {
                using var fbd = new FolderBrowserDialog { Description = "Quellordner auswählen" };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _sourceFolder = fbd.SelectedPath;
                    txtSourceFile.Text = _sourceFolder;
                    UpdateStatus("Quellordner ausgewählt.");
                }
            }
            else
            {
                using var ofd = new OpenFileDialog
                {
                    Title = "Quelldatei auswählen",
                    Filter = "Alle Dateien (*.*)|*.*"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _sourceFile = ofd.FileName;
                    txtSourceFile.Text = _sourceFile;
                    txtBaseName.Text = System.IO.Path.GetFileNameWithoutExtension(_sourceFile);
                    UpdateStatus("Quelldatei ausgewählt.");
                }
            }
        }

        private void btnSelectOutput_Click(object? sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog { Description = "Ausgabeordner auswählen" };
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                _outputDirectory = fbd.SelectedPath;
                txtOutputDir.Text = _outputDirectory;
                UpdateStatus("Ausgabeordner ausgewählt.");
            }
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            if (_folderMode)
            {
                if (string.IsNullOrEmpty(_sourceFolder) || !Directory.Exists(_sourceFolder))
                {
                    MessageBox.Show("Bitte einen gültigen Quellordner auswählen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_sourceFile) || !System.IO.File.Exists(_sourceFile))
                {
                    MessageBox.Show("Bitte eine gültige Quelldatei auswählen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _outputBaseName = txtBaseName.Text.Trim();
                if (string.IsNullOrEmpty(_outputBaseName))
                {
                    MessageBox.Show("Bitte einen Basis-Dateinamen eingeben.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (string.IsNullOrEmpty(_outputDirectory) || !Directory.Exists(_outputDirectory))
            {
                MessageBox.Show("Bitte einen gültigen Ausgabeordner auswählen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _intervalMs = GetSelectedIntervalMs();
            _backupTimer.Interval = _intervalMs;
            cmbInterval.Enabled = false;
            rbFile.Enabled = false;
            rbFolder.Enabled = false;

            DoBackup();
            _backupTimer.Start();
            _progressTimer.Start();

            btnStart.Enabled = false;
            btnStop.Enabled = true;

            string intervalLabel = cmbInterval.SelectedItem?.ToString() ?? "?";
            UpdateStatus($"Backup läuft. Intervall: {intervalLabel}.");
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            _backupTimer.Stop();
            _progressTimer.Stop();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            cmbInterval.Enabled = true;
            rbFile.Enabled = true;
            rbFolder.Enabled = true;

            progressBar.Value = 0;
            lblCountdown.Text = "–";
            UpdateStatus("Backup gestoppt.");
        }

        private void BackupTimer_Tick(object? sender, EventArgs e) => DoBackup();

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            if (_intervalMs <= 0) return;

            double elapsed = (DateTime.Now - _lastBackupTime).TotalMilliseconds;
            double remaining = Math.Max(_intervalMs - elapsed, 0);

            progressBar.Value = Math.Clamp((int)(elapsed / _intervalMs * 100), 0, 100);

            TimeSpan r = TimeSpan.FromMilliseconds(remaining);
            lblCountdown.Text = r.TotalHours >= 1
                ? $"Nächstes Backup in {(int)r.TotalHours}h {r.Minutes:D2}m {r.Seconds:D2}s"
                : r.TotalMinutes >= 1
                    ? $"Nächstes Backup in {(int)r.TotalMinutes}m {r.Seconds:D2}s"
                    : $"Nächstes Backup in {r.Seconds}s";
        }

        private void DoBackup()
        {
            _lastBackupTime = DateTime.Now;
            progressBar.Value = 0;

            if (_folderMode)
                DoFolderBackup();
            else
                DoFileBackup();
        }

        private void DoFileBackup()
        {
            try
            {
                string ext = System.IO.Path.GetExtension(_sourceFile);
                string timestamp = DateTime.Now.ToString("dd_MM_yyyy_HH_mm");
                string destFileName = $"{_outputBaseName}_{timestamp}{ext}";
                string destPath = System.IO.Path.Combine(_outputDirectory, destFileName);

                CopyLockedFile(_sourceFile, destPath);

                string msg = $"[{DateTime.Now:HH:mm:ss}] Backup: {destFileName}";
                UpdateStatus(msg);
                AppendLog(msg);
            }
            catch (Exception ex)
            {
                string msg = $"[{DateTime.Now:HH:mm:ss}] Fehler: {ex.Message}";
                UpdateStatus(msg);
                AppendLog(msg);
            }
        }

        private void DoFolderBackup()
        {
            string timestamp = DateTime.Now.ToString("dd_MM_yyyy_HH_mm");
            string rootFolderName = System.IO.Path.GetFileName(_sourceFolder.TrimEnd(System.IO.Path.DirectorySeparatorChar));
            string destRoot = System.IO.Path.Combine(_outputDirectory, $"{rootFolderName}_{timestamp}");

            int success = 0;
            int failed = 0;

            CopyDirectoryRecursive(_sourceFolder, destRoot, ref success, ref failed);

            string summary = failed == 0
                ? $"[{DateTime.Now:HH:mm:ss}] Ordner-Backup: {success} Datei(en) kopiert → {System.IO.Path.GetFileName(destRoot)}"
                : $"[{DateTime.Now:HH:mm:ss}] Ordner-Backup: {success} OK, {failed} Fehler → {System.IO.Path.GetFileName(destRoot)}";

            UpdateStatus(summary);
            AppendLog(summary);
        }

        private void CopyDirectoryRecursive(string sourceDir, string destDir, ref int success, ref int failed)
        {
            try
            {
                Directory.CreateDirectory(destDir);
            }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Ordner konnte nicht erstellt werden '{destDir}': {ex.Message}");
                failed++;
                return;
            }

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                try
                {
                    string destFile = System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(file));
                    CopyLockedFile(file, destFile);
                    success++;
                }
                catch (Exception ex)
                {
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Fehler bei '{System.IO.Path.GetFileName(file)}': {ex.Message}");
                    failed++;
                }
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = System.IO.Path.GetFileName(subDir);
                string destSubDir = System.IO.Path.Combine(destDir, subDirName);
                CopyDirectoryRecursive(subDir, destSubDir, ref success, ref failed);
            }
        }

        private static void CopyLockedFile(string sourcePath, string destPath)
        {
            using var src = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var dst = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None);
            src.CopyTo(dst);
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired) Invoke(() => lblStatus.Text = message);
            else lblStatus.Text = message;
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired) Invoke(() => lstLog.Items.Insert(0, message));
            else lstLog.Items.Insert(0, message);
        }

        private void txtBaseName_TextChanged(object? sender, EventArgs e)
        {
            _outputBaseName = txtBaseName.Text.Trim();
        }
    }
}