namespace AiFileOrganizer
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetDir = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();

            this.label1.AutoSize = true; this.label1.Location = new System.Drawing.Point(12, 15); this.label1.Text = "Gemini API Key:";
            this.txtApiKey.Location = new System.Drawing.Point(12, 33); this.txtApiKey.Size = new System.Drawing.Size(360, 23);

            this.label2.AutoSize = true; this.label2.Location = new System.Drawing.Point(12, 70); this.label2.Text = "目標根目錄:";
            this.txtTargetDir.Location = new System.Drawing.Point(12, 88); this.txtTargetDir.Size = new System.Drawing.Size(280, 23);
            this.btnBrowse.Location = new System.Drawing.Point(298, 87); this.btnBrowse.Size = new System.Drawing.Size(75, 25); this.btnBrowse.Text = "瀏覽...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            this.label4.AutoSize = true; this.label4.Location = new System.Drawing.Point(12, 125); this.label4.Text = "AI 模型 (推薦 gemini-1.5-pro):";
            this.cmbModel.Location = new System.Drawing.Point(12, 143); this.cmbModel.Size = new System.Drawing.Size(360, 23);

            this.btnSave.Location = new System.Drawing.Point(297, 200); this.btnSave.Size = new System.Drawing.Size(75, 30); this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.ClientSize = new System.Drawing.Size(384, 250);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowse); this.Controls.Add(this.txtTargetDir); this.Controls.Add(this.label2);
            this.Controls.Add(this.txtApiKey); this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbModel); this.Controls.Add(this.label4);
            this.Name = "SettingsForm"; this.Text = "設定";
            this.ResumeLayout(false); this.PerformLayout();
        }
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetDir;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbModel;
    }
}
