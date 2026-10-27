namespace DocumentWatchdog
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFileLabel;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblIntervalLabel;
        private System.Windows.Forms.ComboBox cmbInterval;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblFileSize;

        private System.Windows.Forms.GroupBox grpNotify;
        private System.Windows.Forms.CheckBox chkSound;
        private System.Windows.Forms.CheckBox chkEmail;

        private System.Windows.Forms.Panel panelEmailSettings;
        private System.Windows.Forms.Label lblSmtpHost;
        private System.Windows.Forms.TextBox txtSmtpHost;
        private System.Windows.Forms.Label lblSmtpPort;
        private System.Windows.Forms.NumericUpDown nudSmtpPort;
        private System.Windows.Forms.CheckBox chkSmtpSsl;
        private System.Windows.Forms.Label lblSmtpUser;
        private System.Windows.Forms.TextBox txtSmtpUser;
        private System.Windows.Forms.Label lblSmtpPass;
        private System.Windows.Forms.TextBox txtSmtpPass;
        private System.Windows.Forms.Label lblEmailTo;
        private System.Windows.Forms.TextBox txtEmailTo;
        private System.Windows.Forms.Button btnTestEmail;

        private System.Windows.Forms.Panel panelAlert;
        private System.Windows.Forms.Label lblAlertTitle;
        private System.Windows.Forms.Label lblAlertMessage;
        private System.Windows.Forms.Button btnDismiss;

        private System.Windows.Forms.Label lblLogLabel;
        private System.Windows.Forms.ListBox lstLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ── Form ──────────────────────────────────────────────────────────
            this.Text = "DocumentWatchdog";
            this.Size = new System.Drawing.Size(720, 780);
            this.MinimumSize = new System.Drawing.Size(720, 780);
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.ForeColor = System.Drawing.Color.FromArgb(220, 220, 230);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            int col1 = 20, col2 = 160, w = 660, inputW = 480;

            // ── Title ─────────────────────────────────────────────────────────
            lblTitle = new System.Windows.Forms.Label
            {
                Text = "🐕 DocumentWatchdog",
                Font = new System.Drawing.Font("Segoe UI", 18f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(80, 200, 240),
                Location = new System.Drawing.Point(col1, 16),
                Size = new System.Drawing.Size(w, 40),
                AutoSize = false
            };

            // ── File path ─────────────────────────────────────────────────────
            lblFileLabel = MakeLabel("Datei:", col1, 72);
            txtFilePath = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(col2, 68),
                Size = new System.Drawing.Size(390, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                ReadOnly = true
            };
            btnBrowse = MakeButton("📂 Durchsuchen", col2 + 396, 65, 120, 30);
            btnBrowse.Click += btnBrowse_Click;

            // ── Interval ──────────────────────────────────────────────────────
            lblIntervalLabel = MakeLabel("Prüfintervall:", col1, 108);
            cmbInterval = new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(col2, 104),
                Size = new System.Drawing.Size(200, 24),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };

            // ── Status / Size ─────────────────────────────────────────────────
            lblStatus = new System.Windows.Forms.Label
            {
                Text = "Status: Bereit",
                Location = new System.Drawing.Point(col1, 142),
                Size = new System.Drawing.Size(w / 2, 22),
                ForeColor = System.Drawing.Color.FromArgb(180, 180, 180)
            };
            lblFileSize = new System.Windows.Forms.Label
            {
                Text = "Dateigröße: –",
                Location = new System.Drawing.Point(col1 + w / 2, 142),
                Size = new System.Drawing.Size(w / 2, 22),
                ForeColor = System.Drawing.Color.FromArgb(180, 180, 180)
            };

            // ── Start/Stop ────────────────────────────────────────────────────
            btnStartStop = MakeButton("▶ Starten", col1, 170, 140, 36);
            btnStartStop.Font = new System.Drawing.Font("Segoe UI", 10.5f, System.Drawing.FontStyle.Bold);
            btnStartStop.BackColor = System.Drawing.Color.FromArgb(30, 140, 80);
            btnStartStop.Click += btnStartStop_Click;

            // ── Notifications ─────────────────────────────────────────────────
            grpNotify = new System.Windows.Forms.GroupBox
            {
                Text = "Benachrichtigungen",
                Location = new System.Drawing.Point(col1, 218),
                Size = new System.Drawing.Size(w, 56),
                ForeColor = System.Drawing.Color.FromArgb(140, 200, 255)
            };
            chkSound = new System.Windows.Forms.CheckBox
            {
                Text = "🔊 Alarmton (Windows)",
                Location = new System.Drawing.Point(12, 22),
                Size = new System.Drawing.Size(200, 22),
                Checked = true,
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230)
            };
            chkEmail = new System.Windows.Forms.CheckBox
            {
                Text = "📧 E-Mail-Benachrichtigung",
                Location = new System.Drawing.Point(220, 22),
                Size = new System.Drawing.Size(220, 22),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230)
            };
            chkEmail.CheckedChanged += chkEmail_CheckedChanged;
            grpNotify.Controls.Add(chkSound);
            grpNotify.Controls.Add(chkEmail);

            // ── Email Settings Panel ──────────────────────────────────────────
            panelEmailSettings = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(col1, 280),
                Size = new System.Drawing.Size(w, 200),
                Visible = false,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(35, 35, 50)
            };

            int ex = 10, ey = 10, labelW = 130, inputEW = 260, portW = 70;

            var lblSmtpTitle = new System.Windows.Forms.Label
            {
                Text = "SMTP-Einstellungen",
                Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(ex, ey),
                Size = new System.Drawing.Size(300, 20),
                ForeColor = System.Drawing.Color.FromArgb(140, 200, 255)
            };

            lblSmtpHost = MakeLabel("SMTP-Server:", ex, ey + 28);
            txtSmtpHost = MakeTextBox(ex + labelW, ey + 24, inputEW, "z.B. smtp.gmail.com");

            lblSmtpPort = MakeLabel("Port:", ex + labelW + inputEW + 10, ey + 28);
            nudSmtpPort = new System.Windows.Forms.NumericUpDown
            {
                Location = new System.Drawing.Point(ex + labelW + inputEW + 50, ey + 24),
                Size = new System.Drawing.Size(portW, 24),
                Minimum = 1,
                Maximum = 65535,
                Value = 587,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230)
            };
            chkSmtpSsl = new System.Windows.Forms.CheckBox
            {
                Text = "SSL/TLS",
                Location = new System.Drawing.Point(ex + labelW + inputEW + 130, ey + 26),
                Size = new System.Drawing.Size(80, 20),
                Checked = true,
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230)
            };

            lblSmtpUser = MakeLabel("Benutzername:", ex, ey + 60);
            txtSmtpUser = MakeTextBox(ex + labelW, ey + 56, inputEW, "E-Mail-Adresse");

            lblSmtpPass = MakeLabel("Passwort:", ex, ey + 94);
            txtSmtpPass = MakeTextBox(ex + labelW, ey + 90, inputEW, "App-Passwort");
            txtSmtpPass.PasswordChar = '●';

            lblEmailTo = MakeLabel("Empfänger:", ex, ey + 128);
            txtEmailTo = MakeTextBox(ex + labelW, ey + 124, inputEW, "empfaenger@example.com");

            btnTestEmail = MakeButton("✉ Testmail senden", ex + labelW + inputEW + 10, ey + 122, 160, 28);
            btnTestEmail.Click += btnTestEmail_Click;

            panelEmailSettings.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblSmtpTitle, lblSmtpHost, txtSmtpHost, lblSmtpPort, nudSmtpPort, chkSmtpSsl,
                lblSmtpUser, txtSmtpUser, lblSmtpPass, txtSmtpPass,
                lblEmailTo, txtEmailTo, btnTestEmail
            });

            // ── Alert Panel ───────────────────────────────────────────────────
            panelAlert = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(col1, 490),
                Size = new System.Drawing.Size(w, 110),
                BackColor = System.Drawing.Color.FromArgb(160, 0, 0),
                Visible = false
            };
            lblAlertTitle = new System.Windows.Forms.Label
            {
                Text = "🚨 ALARM!",
                Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(10, 8),
                Size = new System.Drawing.Size(w - 20, 30),
                AutoSize = false
            };
            lblAlertMessage = new System.Windows.Forms.Label
            {
                Text = "",
                Font = new System.Drawing.Font("Segoe UI", 9.5f),
                ForeColor = System.Drawing.Color.FromArgb(255, 220, 220),
                Location = new System.Drawing.Point(10, 42),
                Size = new System.Drawing.Size(w - 160, 60),
                AutoSize = false
            };
            btnDismiss = MakeButton("✔ Alarm quittieren", w - 150, 40, 140, 34);
            btnDismiss.BackColor = System.Drawing.Color.FromArgb(80, 0, 0);
            btnDismiss.ForeColor = System.Drawing.Color.White;
            btnDismiss.Click += btnDismiss_Click;

            panelAlert.Controls.Add(lblAlertTitle);
            panelAlert.Controls.Add(lblAlertMessage);
            panelAlert.Controls.Add(btnDismiss);

            // ── Log ───────────────────────────────────────────────────────────
            lblLogLabel = MakeLabel("Ereignisprotokoll:", col1, 610);
            lstLog = new System.Windows.Forms.ListBox
            {
                Location = new System.Drawing.Point(col1, 630),
                Size = new System.Drawing.Size(w, 110),
                BackColor = System.Drawing.Color.FromArgb(20, 20, 30),
                ForeColor = System.Drawing.Color.FromArgb(160, 220, 160),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Consolas", 8.5f)
            };

            // ── Add to Form ───────────────────────────────────────────────────
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,
                lblFileLabel, txtFilePath, btnBrowse,
                lblIntervalLabel, cmbInterval,
                lblStatus, lblFileSize,
                btnStartStop,
                grpNotify,
                panelEmailSettings,
                panelAlert,
                lblLogLabel, lstLog
            });

            this.ResumeLayout(false);
        }

        // ── Factory helpers ───────────────────────────────────────────────────
        private System.Windows.Forms.Label MakeLabel(string text, int x, int y)
            => new System.Windows.Forms.Label
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(135, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

        private System.Windows.Forms.TextBox MakeTextBox(int x, int y, int width, string placeholder)
        {
            var tb = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(width, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                PlaceholderText = placeholder
            };
            return tb;
        }

        private System.Windows.Forms.Button MakeButton(string text, int x, int y, int w, int h)
            => new System.Windows.Forms.Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                BackColor = System.Drawing.Color.FromArgb(55, 70, 100),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
    }
}