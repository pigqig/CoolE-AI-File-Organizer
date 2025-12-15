using System.IO;
using System.Text.Json;

public class AppSettings
{
    public string ApiKey { get; set; } = "";
    public string TargetRootDirectory { get; set; } = @"D:\我的文件\AI整理箱";
    public string AiSystemPrompt { get; set; } = "你是一個檔案整理專家。";
    public string ModelName { get; set; } = "gemini-2.5-pro";

    private static string _filePath = "appsettings.json";

    public static AppSettings Load()
    {
        if (!File.Exists(_filePath)) return new AppSettings();
        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(_filePath, json);
    }
}
