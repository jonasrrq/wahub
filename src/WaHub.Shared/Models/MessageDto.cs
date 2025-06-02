namespace WaHub.Shared.Models;

public class MessageDto
{
    public string Id { get; set; } = "";
    public string InstanceId { get; set; } = "";
    public string InstanceName { get; set; } = "";
    public string RecipientPhone { get; set; } = "";
    public string RecipientName { get; set; } = "";
    public string Content { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public string MediaUrl { get; set; } = "";
}
