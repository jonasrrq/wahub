using WaHub.Shared.Models;

namespace WaHub.Shared.Services;

public class NotificationMessage
{
    public NotificationType Type { get; set; }
    public string Message { get; set; } = "";
    public int Duration { get; set; } = 5000;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
