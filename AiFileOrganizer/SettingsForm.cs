using System;
using System.Windows.Forms;

namespace AiFileOrganizer
{
    public partial class SettingsForm : Form
    {
        private AppSettings _settings;

        public SettingsForm()
        {
            InitializeComponent();
            InitializeModels();
            LoadSettings();
        }

        private void InitializeModels()
        {
            cmbModel.Items.Clear();
            cmbModel.Items.Add("gemini-2.5-pro");
            cmbModel.Items.Add("gemini-2.5-flash");
            cmbModel.Items.Add("gemini-2.5-flash-exp");
            cmbModel.DropDownStyle = ComboBoxStyle.DropDown; 
        }

        private void LoadSettings()
        {
            _settings = AppSettings.Load();
            txtApiKey.Text = _settings.ApiKey;
            txtTargetDir.Text = _settings.TargetRootDirectory;
            cmbModel.Text = string.IsNullOrEmpty(_settings.ModelName) ? "gemini-1.5-pro" : _settings.ModelName;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK) txtTargetDir.Text = dialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.ApiKey = txtApiKey.Text;
            _settings.TargetRootDirectory = txtTargetDir.Text;
            _settings.ModelName = cmbModel.Text.Trim();
            try
            {
                _settings.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("儲存失敗: " + ex.Message); }
        }
    }
}
