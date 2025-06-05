using Microsoft.AspNetCore.Components;

namespace WaHub.Client.Pages;

public partial class Dashboard : IDisposable
{
    private int clickCount = 0;
    private bool showMessage = false;

    // Monitor de estado en tiempo real
    private int onlineUsers = 127;
    private int messagesPerSecond = 15;
    private int serverLoad = 42;
    private int queueSize = 8;
    private bool isMonitoringActive = false;
    private Timer? monitoringTimer;
    private bool isDisposed = false;

    private void TestInteractivity()
    {
        clickCount++;
        showMessage = true;
        StateHasChanged();
    }

    private void ResetCounter()
    {
        clickCount = 0;
        showMessage = false;
        StateHasChanged();
    }
    private void ToggleMonitoring(ChangeEventArgs e)
    {
        isMonitoringActive = (bool)(e.Value ?? false);

        if (isMonitoringActive)
        {
            StartMonitoring();
        }
        else
        {
            StopMonitoring();
        }

        StateHasChanged();
    }

    private void StartMonitoring()
    {
        // Solo iniciar si no está disposed
        if (!isDisposed)
        {
            monitoringTimer = new Timer(UpdateMetrics, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
        }
    }

    private void StopMonitoring()
    {
        monitoringTimer?.Dispose();
        monitoringTimer = null;
    }

    private void UpdateMetrics(object? state)
    {
        // Verificar si el componente está disposed antes de actualizar
        if (isDisposed)
        {
            StopMonitoring();
            return;
        }

        try
        {
            var random = new Random();

            // Simular cambios realistas en las métricas
            onlineUsers += random.Next(-5, 8);
            onlineUsers = Math.Max(50, Math.Min(200, onlineUsers));

            messagesPerSecond += random.Next(-3, 5);
            messagesPerSecond = Math.Max(5, Math.Min(30, messagesPerSecond));

            serverLoad += random.Next(-8, 10);
            serverLoad = Math.Max(20, Math.Min(90, serverLoad));

            queueSize += random.Next(-2, 4);
            queueSize = Math.Max(0, Math.Min(20, queueSize));

            // Usar InvokeAsync con verificación de disposed
            InvokeAsync(() =>
            {
                if (!isDisposed)
                {
                    StateHasChanged();
                }
            });
        }
        catch (ObjectDisposedException)
        {
            // Si el componente está disposed, detener el timer
            StopMonitoring();
        }
        catch (Exception)
        {
            // En caso de cualquier otro error, detener el monitoreo
            StopMonitoring();
        }
    }

    public void Dispose()
    {
        isDisposed = true;
        StopMonitoring();
    }


}

