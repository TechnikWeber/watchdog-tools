namespace RunningBackup
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.RadioButton rbFile = null!;
        private System.Windows.Forms.RadioButton rbFolder = null!;
        private System.Windows.Forms.Label lblSource = null!;
        private System.Windows.Forms.TextBox txtSourceFile = null!;
        private System.Windows.Forms.Button btnSelectSource = null!;
        private System.Windows.Forms.Label lblOutput = null!;
        private System.Windows.Forms.TextBox txtOutputDir = null!;
        private System.Windows.Forms.Button btnSelectOutput = null!;
        private System.Windows.Forms.Label lblBaseName = null!;
        private System.Windows.Forms.TextBox txtBaseName = null!;
        private System.Windows.Forms.Label lblInterval = null!;
        private System.Windows.Forms.ComboBox cmbInterval = null!;
        private System.Windows.Forms.Button btnStart = null!;
        private System.Windows.Forms.Button btnStop = null!;
        private System.Windows.Forms.Label lblStatus = null!;
        private System.Windows.Forms.ProgressBar progressBar = null!;
        private System.Windows.Forms.Label lblCountdown = null!;
        private System.Windows.Forms.Label lblLog = null!;
        private System.Windows.Forms.ListBox lstLog = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            rbFile = new System.Windows.Forms.RadioButton();
            rbFolder = new System.Windows.Forms.RadioButton();
            lblSource = new System.Windows.Forms.Label();
            txtSourceFile = new System.Windows.Forms.TextBox();
            btnSelectSource = new System.Windows.Forms.Button();
            lblOutput = new System.Windows.Forms.Label();
            txtOutputDir = new System.Windows.Forms.TextBox();
            btnSelectOutput = new System.Windows.Forms.Button();
            lblBaseName = new System.Windows.Forms.Label();
            txtBaseName = new System.Windows.Forms.TextBox();
            lblInterval = new System.Windows.Forms.Label();
            cmbInterval = new System.Windows.Forms.ComboBox();
            btnStart = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            lblCountdown = new System.Windows.Forms.Label();
            lblLog = new System.Windows.Forms.Label();
            lstLog = new System.Windows.Forms.ListBox();

            this.SuspendLayout();

            // Form
            this.Text = "RunningBackup";
            this.ClientSize = new System.Drawing.Size(620, 580);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // rbFile
            rbFile.Text = "Einzelne Datei";
            rbFile.Location = new System.Drawing.Point(12, 15);
            rbFile.Size = new System.Drawing.Size(130, 22);
            rbFile.Checked = true;
            rbFile.CheckedChanged += rbFile_CheckedChanged;

            // rbFolder
            rbFolder.Text = "Ordner";
            rbFolder.Location = new System.Drawing.Point(155, 15);
            rbFolder.Size = new System.Drawing.Size(80, 22);
            rbFolder.CheckedChanged += rbFile_CheckedChanged;

            // lblSource
            lblSource.Text = "Quelldatei:";
            lblSource.Location = new System.Drawing.Point(12, 50);
            lblSource.Size = new System.Drawing.Size(95, 23);
            lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // txtSourceFile
            txtSourceFile.Location = new System.Drawing.Point(110, 50);
            txtSourceFile.Size = new System.Drawing.Size(385, 23);
            txtSourceFile.ReadOnly = true;

            // btnSelectSource
            btnSelectSource.Text = "Datei...";
            btnSelectSource.Location = new System.Drawing.Point(503, 49);
            btnSelectSource.Size = new System.Drawing.Size(90, 25);
            btnSelectSource.Click += btnSelectSource_Click;

            // lblOutput
            lblOutput.Text = "Ausgabeordner:";
            lblOutput.Location = new System.Drawing.Point(12, 90);
            lblOutput.Size = new System.Drawing.Size(95, 23);
            lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // txtOutputDir
            txtOutputDir.Location = new System.Drawing.Point(110, 90);
            txtOutputDir.Size = new System.Drawing.Size(385, 23);
            txtOutputDir.ReadOnly = true;

            // btnSelectOutput
            btnSelectOutput.Text = "Ordner...";
            btnSelectOutput.Location = new System.Drawing.Point(503, 89);
            btnSelectOutput.Size = new System.Drawing.Size(90, 25);
            btnSelectOutput.Click += btnSelectOutput_Click;

            // lblBaseName
            lblBaseName.Text = "Basis-Name:";
            lblBaseName.Location = new System.Drawing.Point(12, 130);
            lblBaseName.Size = new System.Drawing.Size(95, 23);
            lblBaseName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // txtBaseName
            txtBaseName.Location = new System.Drawing.Point(110, 130);
            txtBaseName.Size = new System.Drawing.Size(385, 23);
            txtBaseName.PlaceholderText = "z.B. messung  →  messung_24_02_2026_10_00.csv";
            txtBaseName.TextChanged += txtBaseName_TextChanged;

            // lblInterval
            lblInterval.Text = "Intervall:";
            lblInterval.Location = new System.Drawing.Point(12, 170);
            lblInterval.Size = new System.Drawing.Size(95, 23);
            lblInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // cmbInterval
            cmbInterval.Location = new System.Drawing.Point(110, 170);
            cmbInterval.Size = new System.Drawing.Size(160, 23);
            cmbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // btnStart
            btnStart.Text = "▶ Backup starten";
            btnStart.Location = new System.Drawing.Point(12, 212);
            btnStart.Size = new System.Drawing.Size(150, 35);
            btnStart.BackColor = System.Drawing.Color.FromArgb(0, 150, 0);
            btnStart.ForeColor = System.Drawing.Color.White;
            btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStart.Click += btnStart_Click;

            // btnStop
            btnStop.Text = "⏹ Backup stoppen";
            btnStop.Location = new System.Drawing.Point(175, 212);
            btnStop.Size = new System.Drawing.Size(150, 35);
            btnStop.BackColor = System.Drawing.Color.FromArgb(180, 0, 0);
            btnStop.ForeColor = System.Drawing.Color.White;
            btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStop.Enabled = false;
            btnStop.Click += btnStop_Click;

            // lblStatus
            lblStatus.Text = "Bereit.";
            lblStatus.Location = new System.Drawing.Point(12, 260);
            lblStatus.Size = new System.Drawing.Size(590, 20);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            // progressBar
            progressBar.Location = new System.Drawing.Point(12, 285);
            progressBar.Size = new System.Drawing.Size(590, 22);
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            // lblCountdown
            lblCountdown.Text = "–";
            lblCountdown.Location = new System.Drawing.Point(12, 311);
            lblCountdown.Size = new System.Drawing.Size(590, 20);
            lblCountdown.Font = new System.Drawing.Font("Consolas", 9f);
            lblCountdown.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);

            // lblLog
            lblLog.Text = "Protokoll:";
            lblLog.Location = new System.Drawing.Point(12, 338);
            lblLog.Size = new System.Drawing.Size(80, 20);

            // lstLog
            lstLog.Location = new System.Drawing.Point(12, 361);
            lstLog.Size = new System.Drawing.Size(590, 205);
            lstLog.Font = new System.Drawing.Font("Consolas", 9f);

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                rbFile, rbFolder,
                lblSource, txtSourceFile, btnSelectSource,
                lblOutput, txtOutputDir, btnSelectOutput,
                lblBaseName, txtBaseName,
                lblInterval, cmbInterval,
                btnStart, btnStop,
                lblStatus,
                progressBar, lblCountdown,
                lblLog, lstLog
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}