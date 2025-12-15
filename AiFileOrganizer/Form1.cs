using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AiFileOrganizer
{
    public partial class Form1 : Form
    {
        private GeminiService _aiService;
        private AppSettings _settings;
        private Stack<List<FileOperation>> _undoStack = new Stack<List<FileOperation>>();

        public class FileOperation { public string OriginalPath { get; set; } public string NewPath { get; set; } }

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            ReloadSettings();
            notifyIcon1.Icon = SystemIcons.Application;
            this.Icon = SystemIcons.Application;
        }

        private void InitializeCustomComponents()
        {
            this.AllowDrop = true;
            this.DragEnter += (s, e) => { if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; };
            this.DragDrop += Form1_DragDrop;
            
            btnSettings.Click += (s, e) => OpenSettings();
            btnUndo.Click += BtnUndo_Click;
            btnUndo.Enabled = false;

            tsmiOpen.Click += (s, e) => RestoreWindow();
            tsmiSettings.Click += (s, e) => OpenSettings();
            tsmiExit.Click += (s, e) => Application.Exit();
            notifyIcon1.DoubleClick += (s, e) => RestoreWindow();
            this.Resize += Form1_Resize;
        }

        private void ReloadSettings()
        {
            _settings = AppSettings.Load();
            _aiService = new GeminiService(_settings.ApiKey, _settings.ModelName);
            Log($"系統就緒。使用模型: {_settings.ModelName}");
        }

        private void OpenSettings()
        {
            using (var form = new SettingsForm())
            {
                if (form.ShowDialog() == DialogResult.OK) ReloadSettings();
            }
        }

        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;

            Log("==========================================");
            Log($"[批次啟動] 收到 {files.Length} 個檔案。");
            Log("正在分批發送給 AI 分析 (每批 30 個)，請稍候...");
            
            lblStatus.Text = "AI 深度分析中 (檔案較多，請耐心等待)...";
            progressBar1.Style = ProgressBarStyle.Marquee; 
            
            List<FileOperation> currentBatchOps = new List<FileOperation>();

            try
            {
                if (string.IsNullOrEmpty(_settings.ApiKey)) throw new Exception("請先設定 API Key！");

                // --- 呼叫改版後的 AI 服務 ---
                // 這裡現在會花比較久時間 (因為要跑好幾輪 API)，但不會 Timeout
                List<AiResult> aiSuggestions = await _aiService.AnalyzeBatchFilesAsync(files.ToList());

                Log($"[AI 分析完成] 共取得 {aiSuggestions.Count} 個建議。開始移動檔案...");
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Maximum = aiSuggestions.Count;
                progressBar1.Value = 0;

                int processIndex = 0;
                foreach (var suggestion in aiSuggestions)
                {
                    try
                    {
                        ProcessMove(suggestion, currentBatchOps);
                    }
                    catch (Exception moveEx)
                    {
                        Log($"[移動失敗] {suggestion.NewFileName}: {moveEx.Message}");
                    }
                    processIndex++;
                    progressBar1.Value = processIndex;
                }

                if (currentBatchOps.Count > 0)
                {
                    _undoStack.Push(currentBatchOps);
                    btnUndo.Enabled = true;
                    btnUndo.Text = $"還原上一步 ({_undoStack.Count})";
                    Log($"任務結束！成功搬移 {currentBatchOps.Count} / {files.Length} 個檔案。");
                }
                else
                {
                    Log("警告：沒有檔案被移動，請檢查 Log 看是否 AI 解析失敗。");
                }

            }
            catch (Exception ex)
            {
                Log($"[嚴重錯誤] {ex.Message}");
                FileLogger.LogError("Main Process Error", ex);
                MessageBox.Show($"執行失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblStatus.Text = "就緒";
                progressBar1.Value = 0;
                progressBar1.Style = ProgressBarStyle.Blocks;
            }
        }

        private void ProcessMove(AiResult result, List<FileOperation> batchLog)
        {
            string sourceFile = result.OriginalFilePath;
            if (!File.Exists(sourceFile)) return;

            string targetFolder = Path.Combine(_settings.TargetRootDirectory, result.FolderPath);
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

            string targetPath = Path.Combine(targetFolder, result.NewFileName);
            
            int c = 1;
            while (File.Exists(targetPath))
            {
                string name = Path.GetFileNameWithoutExtension(result.NewFileName);
                string ext = Path.GetExtension(result.NewFileName);
                targetPath = Path.Combine(targetFolder, $"{name}_{c}{ext}");
                c++;
            }

            File.Move(sourceFile, targetPath);
            batchLog.Add(new FileOperation { OriginalPath = sourceFile, NewPath = targetPath });
            
            Log($"[OK] {Path.GetFileName(sourceFile)} -> {result.FolderPath}");
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (_undoStack.Count == 0) return;
            var lastBatch = _undoStack.Pop();
            foreach (var op in lastBatch)
            {
                try
                {
                    if (File.Exists(op.NewPath))
                    {
                        string dir = Path.GetDirectoryName(op.OriginalPath);
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        File.Move(op.NewPath, op.OriginalPath);
                        Log($"還原: {Path.GetFileName(op.OriginalPath)}");
                    }
                }
                catch (Exception ex) { Log($"還原失敗: {op.NewPath}"); }
            }
            btnUndo.Enabled = _undoStack.Count > 0;
            btnUndo.Text = _undoStack.Count > 0 ? $"還原上一步 ({_undoStack.Count})" : "還原上一步";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                notifyIcon1.BalloonTipTitle = "酷意-AI";
                notifyIcon1.BalloonTipText = "背景執行中...";
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void RestoreWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void Log(string msg) { lstLog.Items.Add(msg); lstLog.TopIndex = lstLog.Items.Count - 1; }
    }
}
