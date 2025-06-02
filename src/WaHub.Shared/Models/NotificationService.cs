using WaHub.Shared.Services;

namespace WaHub.Shared.Models;

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
