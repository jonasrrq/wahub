namespace WaHub.Shared.Models.Instances;

public class InstanceDto
{
    public bool Connected { get; set; }
    public string Events { get; set; } = "";
    public int Expiration { get; set; }
    public string Id { get; set; } = "";
    public string Jid { get; set; } = "";
    public bool LoggedIn { get; set; }
    public string Name { get; set; } = "";
    public ProxyConfig Proxy_Config { get; set; } = new();
    public string Proxy_Url { get; set; } = "";
    public string Qrcode { get; set; } = "";
    public S3Config S3_Config { get; set; } = new();
    public string Token { get; set; } = "";
    public string Webhook { get; set; } = "";
}
