namespace WaHub.Shared.Models;

public class NewMessageDto
{
    public string InstanceId { get; set; } = "";
    public string RecipientPhone { get; set; } = "";
    public string Content { get; set; } = "";
    public string MediaUrl { get; set; } = "";
    public bool IsScheduled { get; set; } = false;
    public DateTime? ScheduledAt { get; set; }
}
