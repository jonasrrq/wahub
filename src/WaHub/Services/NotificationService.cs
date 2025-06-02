namespace WaHub.Services;

public class NotificationService
{
    public event Action<NotificationMessage>? OnNotification;

    public void ShowSuccess(string message, int duration = 5000)
    {
        OnNotification?.Invoke(new NotificationMessage
        {
            Type = NotificationType.Success,
            Message = message,
            Duration = duration
        });
    }

    public void ShowError(string message, int duration = 7000)
    {
        OnNotification?.Invoke(new NotificationMessage
        {
            Type = NotificationType.Error,
            Message = message,
            Duration = duration
        });
    }

    public void ShowWarning(string message, int duration = 6000)
    {
        OnNotification?.Invoke(new NotificationMessage
        {
            Type = NotificationType.Warning,
            Message = message,
            Duration = duration
        });
    }

    public void ShowInfo(string message, int duration = 5000)
    {
        OnNotification?.Invoke(new NotificationMessage
        {
            Type = NotificationType.Info,
            Message = message,
            Duration = duration
        });
    }
}

public class NotificationMessage
{
    public NotificationType Type { get; set; }
    public string Message { get; set; } = "";
    public int Duration { get; set; } = 5000;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

public enum NotificationType
{
    Success,
    Error,
    Warning,
    Info
}
