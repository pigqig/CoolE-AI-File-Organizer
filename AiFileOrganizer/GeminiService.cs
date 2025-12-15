using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AiFileOrganizer;

public class GeminiService
{
    private string _apiKey;
    private string _modelName;
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/";
    
    // [設定] 每次處理的檔案數量 (避免 AI 腦容量過載或回應超時)
    private const int BATCH_SIZE = 30; 

    public GeminiService(string apiKey, string modelName)
    {
        _apiKey = apiKey;
        _modelName = string.IsNullOrEmpty(modelName) ? "gemini-2.5-pro" : modelName;
    }

    // [核心] 自動分批處理邏輯
    public async Task<List<AiResult>> AnalyzeBatchFilesAsync(List<string> filePaths)
    {
        List<AiResult> allResults = new List<AiResult>();
        int totalFiles = filePaths.Count;

        FileLogger.Log($"準備處理 {totalFiles} 個檔案，將分為 {(int)Math.Ceiling((double)totalFiles / BATCH_SIZE)} 批次執行...");

        // 分批迴圈
        for (int i = 0; i < totalFiles; i += BATCH_SIZE)
        {
            // 取出這一批的檔案 (例如第 0-30 個，第 30-60 個...)
            var currentBatchFiles = filePaths.Skip(i).Take(BATCH_SIZE).ToList();
            
            FileLogger.Log($"正在處理第 {i + 1} 到 {i + currentBatchFiles.Count} 個檔案...");

            try 
            {
                // 呼叫 API 處理這一小批
                var batchResults = await ProcessSingleBatch(currentBatchFiles);
                allResults.AddRange(batchResults);
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"批次 ({i}-{i+BATCH_SIZE}) 處理失敗，跳過此批。", ex);
                // 即使某一頁失敗，也不要讓整個程式崩潰，繼續做下一頁
            }

            // 稍微休息一下，避免連續呼叫觸發 Rate Limit (選用)
            await Task.Delay(500);
        }

        return allResults;
    }

    // 處理單一小批次
    private async Task<List<AiResult>> ProcessSingleBatch(List<string> batchFiles)
    {
        var fileListInfo = batchFiles.Select(f => $"{Path.GetFileName(f)}").ToList();
        string filesJson = JsonSerializer.Serialize(fileListInfo);

        string prompt = $@"
            我會提供一個檔案清單。請根據「檔名」推測內容並進行分類。
            
            【輸入檔案列表】:
            {filesJson}

            【規則】:
            1. 財務/發票 -> 財務\年份
            2. 圖片/照片 -> 多媒體\圖片
            3. 應用程式/安裝檔 -> 應用程式
            4. 程式碼 -> 專案開發
            5. 無法判斷 -> 未分類

            【嚴格回傳格式 (JSON Array Only)】:
            [
              {{
                ""originalName"": ""原始檔名"",
                ""newFileName"": ""建議新檔名"",
                ""folderPath"": ""建議分類資料夾"",
                ""summary"": ""簡短理由""
              }}
            ]
            不要 Markdown，只回傳純 JSON。
        ";

        var responseString = await CallGeminiApi(prompt);
        return ParseBatchResponse(responseString, batchFiles);
    }

    private async Task<string> CallGeminiApi(string promptText)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = promptText } } }
            }
        };

        using (var client = new HttpClient())
        {
            // [關鍵修正] 設定超時時間為 5 分鐘 (預設只有 100秒)
            client.Timeout = TimeSpan.FromMinutes(5);

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            string url = $"{BaseUrl}{_modelName}:generateContent?key={_apiKey}";
            
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode} - {responseString}");
            }

            return responseString;
        }
    }

    private List<AiResult> ParseBatchResponse(string rawApiResponse, List<string> originalFilePaths)
    {
        try
        {
            string aiText = "";
            using (JsonDocument doc = JsonDocument.Parse(rawApiResponse))
            {
                var root = doc.RootElement;
                if(root.TryGetProperty("candidates", out var candidates))
                {
                    aiText = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                }
            }

            string cleanJson = CleanJsonString(aiText);
            var batchResults = JsonSerializer.Deserialize<List<BatchItem>>(cleanJson);
            
            List<AiResult> finalResults = new List<AiResult>();
            foreach (var item in batchResults)
            {
                // 這裡比對檔名時，要忽略大小寫以增加容錯率
                string fullPath = originalFilePaths.FirstOrDefault(p => Path.GetFileName(p).Equals(item.originalName, StringComparison.OrdinalIgnoreCase));
                
                // 如果 AI 偷懶沒回傳完整的檔名，嘗試模糊比對
                if (fullPath == null)
                {
                     fullPath = originalFilePaths.FirstOrDefault(p => p.Contains(item.originalName));
                }

                if (fullPath != null)
                {
                    finalResults.Add(new AiResult
                    {
                        OriginalFilePath = fullPath,
                        NewFileName = item.newFileName,
                        FolderPath = item.folderPath,
                        Summary = item.summary
                    });
                }
            }
            return finalResults;
        }
        catch (Exception ex)
        {
            FileLogger.LogError("JSON Parsing Failed", ex);
            FileLogger.Log($"Raw Response: {rawApiResponse}", "DEBUG");
            // 回傳空清單，避免整批失敗
            return new List<AiResult>();
        }
    }

    private string CleanJsonString(string text)
    {
        if (string.IsNullOrEmpty(text)) return "[]";
        int startIndex = text.IndexOf('[');
        if (startIndex == -1) return "[]";
        int endIndex = text.LastIndexOf(']');
        if (endIndex == -1 || endIndex < startIndex) return "[]";
        return text.Substring(startIndex, endIndex - startIndex + 1);
    }

    private class BatchItem
    {
        public string originalName { get; set; }
        public string newFileName { get; set; }
        public string folderPath { get; set; }
        public string summary { get; set; }
    }
}
