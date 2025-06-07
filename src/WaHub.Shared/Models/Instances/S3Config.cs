namespace WaHub.Shared.Models.Instances;

public class S3Config
{
    public string Access_Key { get; set; } = "";
    public string Bucket { get; set; } = "";
    public bool Enabled { get; set; }
    public string Endpoint { get; set; } = "";
    public string Media_Delivery { get; set; } = "";
    public bool Path_Style { get; set; }
    public string Public_Url { get; set; } = "";
    public string Region { get; set; } = "";
    public int Retention_Days { get; set; }
}
