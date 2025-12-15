using System.Text.Json.Serialization;

public class AiResult
{
    [JsonIgnore]
    public string OriginalFilePath { get; set; }

    [JsonPropertyName("newFileName")]
    public string NewFileName { get; set; }

    [JsonPropertyName("folderPath")]
    public string FolderPath { get; set; }
    
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
}
