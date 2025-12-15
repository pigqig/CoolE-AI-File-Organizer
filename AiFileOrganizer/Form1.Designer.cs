namespace AiFileOrganizer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lstLog = new ListBox();
            progressBar1 = new ProgressBar();
            lblStatus = new Label();
            btnUndo = new Button();
            btnSettings = new Button();
            label1 = new Label();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsmiOpen = new ToolStripMenuItem();
            tsmiSettings = new ToolStripMenuItem();
            tsmiExit = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lstLog
            // 
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 60);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(560, 289);
            lstLog.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 360);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(560, 23);
            progressBar1.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 390);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(55, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "準備就緒";
            // 
            // btnUndo
            // 
            btnUndo.Location = new Point(416, 12);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(156, 30);
            btnUndo.TabIndex = 3;
            btnUndo.Text = "還原上一步";
            btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(335, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(75, 30);
            btnSettings.TabIndex = 4;
            btnSettings.Text = "設定";
            btnSettings.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 5;
            label1.Text = "酷意-AI (批次版)";
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "酷意-AI";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { tsmiOpen, tsmiSettings, tsmiExit });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(135, 70);
            // 
            // tsmiOpen
            // 
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.Size = new Size(134, 22);
            tsmiOpen.Text = "開啟主視窗";
            // 
            // tsmiSettings
            // 
            tsmiSettings.Name = "tsmiSettings";
            tsmiSettings.Size = new Size(134, 22);
            tsmiSettings.Text = "設定";
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(134, 22);
            tsmiExit.Text = "結束程式";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 421);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnUndo);
            Controls.Add(lblStatus);
            Controls.Add(progressBar1);
            Controls.Add(lstLog);
            Name = "Form1";
            Text = "酷意-AI 智慧檔案整理 ";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
    }
}
