namespace ProgramWatchdog
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblProcessListLabel;
        private System.Windows.Forms.Label lblProcessCount;
        private System.Windows.Forms.Label lblSelectedProcess;
        private System.Windows.Forms.ListBox lstProcesses;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblIntervalLabel;
        private System.Windows.Forms.ComboBox cmbInterval;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblStatus;

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
            this.Text = "ProgramWatchdog";
            this.Size = new System.Drawing.Size(780, 900);
            this.MinimumSize = new System.Drawing.Size(780, 900);
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.ForeColor = System.Drawing.Color.FromArgb(220, 220, 230);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            int col1 = 20, col2 = 160, w = 720;

            // ── Title ─────────────────────────────────────────────────────────
            lblTitle = new System.Windows.Forms.Label
            {
                Text = "🐕 ProgramWatchdog",
                Font = new System.Drawing.Font("Segoe UI", 18f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(255, 160, 60),
                Location = new System.Drawing.Point(col1, 16),
                Size = new System.Drawing.Size(w, 40),
                AutoSize = false
            };

            // ── Process List ──────────────────────────────────────────────────
            lblProcessListLabel = new System.Windows.Forms.Label
            {
                Text = "Laufendes Programm auswählen:",
                Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(140, 200, 255),
                Location = new System.Drawing.Point(col1, 66),
                Size = new System.Drawing.Size(400, 20)
            };

            btnRefresh = MakeButton("🔄 Aktualisieren", col1 + 460, 62, 140, 26);
            btnRefresh.Click += btnRefresh_Click;

            lblProcessCount = new System.Windows.Forms.Label
            {
                Text = "Lädt …",
                Location = new System.Drawing.Point(col1 + 614, 68),
                Size = new System.Drawing.Size(w - 614, 20),
                ForeColor = System.Drawing.Color.FromArgb(120, 130, 150),
                Font = new System.Drawing.Font("Segoe UI", 8f)
            };

            lstProcesses = new System.Windows.Forms.ListBox
            {
                Location = new System.Drawing.Point(col1, 92),
                Size = new System.Drawing.Size(w, 200),
                BackColor = System.Drawing.Color.FromArgb(22, 22, 34),
                ForeColor = System.Drawing.Color.FromArgb(200, 220, 200),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Consolas", 9f),
                ScrollAlwaysVisible = true
            };
            lstProcesses.SelectedIndexChanged += lstProcesses_SelectedIndexChanged;

            lblSelectedProcess = new System.Windows.Forms.Label
            {
                Text = "Kein Prozess ausgewählt",
                Location = new System.Drawing.Point(col1, 298),
                Size = new System.Drawing.Size(w, 20),
                ForeColor = System.Drawing.Color.FromArgb(160, 160, 180)
            };

            // ── Interval ──────────────────────────────────────────────────────
            lblIntervalLabel = new System.Windows.Forms.Label
            {
                Text = "Prüfintervall:",
                Location = new System.Drawing.Point(col1, 334),
                Size = new System.Drawing.Size(135, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            cmbInterval = new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(col2, 330),
                Size = new System.Drawing.Size(200, 24),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };

            // ── Status ────────────────────────────────────────────────────────
            lblStatus = new System.Windows.Forms.Label
            {
                Text = "Status: Bereit",
                Location = new System.Drawing.Point(col1, 364),
                Size = new System.Drawing.Size(w, 22),
                ForeColor = System.Drawing.Color.FromArgb(180, 180, 180)
            };

            // ── Start/Stop ────────────────────────────────────────────────────
            btnStartStop = MakeButton("▶ Starten", col1, 392, 150, 36);
            btnStartStop.Font = new System.Drawing.Font("Segoe UI", 10.5f, System.Drawing.FontStyle.Bold);
            btnStartStop.BackColor = System.Drawing.Color.FromArgb(30, 140, 80);
            btnStartStop.Click += btnStartStop_Click;

            // ── Freeze detection info ─────────────────────────────────────────
            var lblHowLabel = new System.Windows.Forms.Label
            {
                Text = "ℹ Einfrieren wird erkannt durch: IsHungAppWindow API · SendMessageTimeout (5s) · Process.Responding",
                Location = new System.Drawing.Point(col1, 435),
                Size = new System.Drawing.Size(w, 18),
                ForeColor = System.Drawing.Color.FromArgb(100, 120, 160),
                Font = new System.Drawing.Font("Segoe UI", 8f)
            };

            // ── Notifications ─────────────────────────────────────────────────
            grpNotify = new System.Windows.Forms.GroupBox
            {
                Text = "Benachrichtigungen",
                Location = new System.Drawing.Point(col1, 458),
                Size = new System.Drawing.Size(w, 54),
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
                Location = new System.Drawing.Point(col1, 518),
                Size = new System.Drawing.Size(w, 200),
                Visible = false,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(35, 35, 50)
            };

            int ex = 10, ey = 10, labelW = 130, inputEW = 260;

            var lblSmtpTitle = new System.Windows.Forms.Label
            {
                Text = "SMTP-Einstellungen",
                Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(ex, ey),
                Size = new System.Drawing.Size(300, 20),
                ForeColor = System.Drawing.Color.FromArgb(140, 200, 255)
            };

            lblSmtpHost = new System.Windows.Forms.Label
            {
                Text = "SMTP-Server:",
                Location = new System.Drawing.Point(ex, ey + 28),
                Size = new System.Drawing.Size(labelW, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtSmtpHost = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(ex + labelW, ey + 24),
                Size = new System.Drawing.Size(inputEW, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                PlaceholderText = "z.B. smtp.gmail.com"
            };

            lblSmtpPort = new System.Windows.Forms.Label
            {
                Text = "Port:",
                Location = new System.Drawing.Point(ex + labelW + inputEW + 10, ey + 28),
                Size = new System.Drawing.Size(40, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            nudSmtpPort = new System.Windows.Forms.NumericUpDown
            {
                Location = new System.Drawing.Point(ex + labelW + inputEW + 50, ey + 24),
                Size = new System.Drawing.Size(70, 24),
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

            lblSmtpUser = new System.Windows.Forms.Label
            {
                Text = "Benutzername:",
                Location = new System.Drawing.Point(ex, ey + 60),
                Size = new System.Drawing.Size(labelW, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtSmtpUser = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(ex + labelW, ey + 56),
                Size = new System.Drawing.Size(inputEW, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                PlaceholderText = "E-Mail-Adresse"
            };

            lblSmtpPass = new System.Windows.Forms.Label
            {
                Text = "Passwort:",
                Location = new System.Drawing.Point(ex, ey + 94),
                Size = new System.Drawing.Size(labelW, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtSmtpPass = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(ex + labelW, ey + 90),
                Size = new System.Drawing.Size(inputEW, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                PlaceholderText = "App-Passwort",
                PasswordChar = '●'
            };

            lblEmailTo = new System.Windows.Forms.Label
            {
                Text = "Empfänger:",
                Location = new System.Drawing.Point(ex, ey + 128),
                Size = new System.Drawing.Size(labelW, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            txtEmailTo = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(ex + labelW, ey + 124),
                Size = new System.Drawing.Size(inputEW, 24),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 60),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 230),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                PlaceholderText = "empfaenger@example.com"
            };

            btnTestEmail = MakeButton("✉ Testmail senden", ex + labelW + inputEW + 10, ey + 122, 160, 28);
            btnTestEmail.Click += btnTestEmail_Click;

            panelEmailSettings.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblSmtpTitle,
                lblSmtpHost, txtSmtpHost,
                lblSmtpPort, nudSmtpPort, chkSmtpSsl,
                lblSmtpUser, txtSmtpUser,
                lblSmtpPass, txtSmtpPass,
                lblEmailTo, txtEmailTo,
                btnTestEmail
            });

            // ── Alert Panel ───────────────────────────────────────────────────
            panelAlert = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(col1, 725),
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
                Size = new System.Drawing.Size(w - 170, 62),
                AutoSize = false
            };
            btnDismiss = MakeButton("✔ Alarm quittieren", w - 150, 40, 142, 34);
            btnDismiss.BackColor = System.Drawing.Color.FromArgb(80, 0, 0);
            btnDismiss.ForeColor = System.Drawing.Color.White;
            btnDismiss.Click += btnDismiss_Click;

            panelAlert.Controls.Add(lblAlertTitle);
            panelAlert.Controls.Add(lblAlertMessage);
            panelAlert.Controls.Add(btnDismiss);

            // ── Log ───────────────────────────────────────────────────────────
            lblLogLabel = new System.Windows.Forms.Label
            {
                Text = "Ereignisprotokoll:",
                Location = new System.Drawing.Point(col1, 842),
                Size = new System.Drawing.Size(135, 22),
                ForeColor = System.Drawing.Color.FromArgb(160, 170, 190)
            };
            lstLog = new System.Windows.Forms.ListBox
            {
                Location = new System.Drawing.Point(col1, 862),
                Size = new System.Drawing.Size(w, 10),
                BackColor = System.Drawing.Color.FromArgb(20, 20, 30),
                ForeColor = System.Drawing.Color.FromArgb(160, 220, 160),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Consolas", 8.5f)
            };

            // ── Add to Form ───────────────────────────────────────────────────
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,
                lblProcessListLabel, btnRefresh, lblProcessCount,
                lstProcesses,
                lblSelectedProcess,
                lblIntervalLabel, cmbInterval,
                lblStatus,
                btnStartStop,
                lblHowLabel,
                grpNotify,
                panelEmailSettings,
                panelAlert,
                lblLogLabel, lstLog
            });

            this.ResumeLayout(false);

            lstLog.Size = new System.Drawing.Size(w, this.ClientSize.Height - 862 - 10);
        }

        // ── Factory helpers ───────────────────────────────────────────────────
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