using Microsoft.JSInterop;

using WaHub.Shared.Models;

using WaHub.Shared.Services;


namespace WaHub.Client.Pages;

public partial class Instances
{
    private readonly ApiService _apiService;
    private readonly NotificationService _notification;
    private readonly IJSRuntime _jSRuntime;
    private List<WhatsAppInstanceDto> instances = new();
    private bool isLoading = true;
    private string? errorMessage;
    private bool showCreateModal = false;
    private bool showQrModal = false;
    private string createInstanceName = "";
    private string createInstancePhone = "";
    private string? qrCodeImage;


    public Instances(ApiService apiService,
        NotificationService notification,
        IJSRuntime jSRuntime)
    {
        _apiService = apiService;
        _notification = notification;
        _jSRuntime = jSRuntime;
    }




    protected override async Task OnInitializedAsync()
    {
        await LoadInstancesAsync();
    }

    private async Task LoadInstancesAsync()
    {
        try
        {
            isLoading = true;
            errorMessage = null;
            var result = await _apiService.GetInstancesAsync();
            instances = result ?? new List<WhatsAppInstanceDto>();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al cargar instancias: {ex.Message}";
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void CreateNewInstance()
    {
        showCreateModal = true;
        createInstanceName = "";
        createInstancePhone = "";
        StateHasChanged();
    }
    private async Task ConfirmCreateInstance()
    {
        if (string.IsNullOrWhiteSpace(createInstanceName) || string.IsNullOrWhiteSpace(createInstancePhone))
        {
            _notification.ShowWarning("Por favor completa todos los campos");
            return;
        }

        try
        {
            var request = new CreateInstanceRequest
            {
                Name = createInstanceName,
                PhoneNumber = createInstancePhone
            };

            var success = await _apiService.CreateInstanceAsync(request);
            if (success)
            {
                showCreateModal = false;
                await LoadInstancesAsync();
                _notification.ShowSuccess($"Instancia '{createInstanceName}' creada exitosamente");
            }
            else
            {
                _notification.ShowError("Error al crear la instancia. Inténtalo de nuevo.");
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error inesperado: {ex.Message}");
        }
    }

    private void CloseCreateModal()
    {
        showCreateModal = false;
        StateHasChanged();
    }
    private async Task ToggleInstance(WhatsAppInstanceDto instance)
    {
        try
        {
            var status = instance.Status == "Conectado" ? "pausada" : "activada";
            var success = await _apiService.ToggleInstanceAsync(instance.Id);
            if (success)
            {
                await LoadInstancesAsync();
                _notification.ShowSuccess($"Instancia '{instance.Name}' {status} correctamente");
            }
            else
            {
                _notification.ShowError("Error al cambiar el estado de la instancia");
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error inesperado: {ex.Message}");
        }
    }
    private async Task DeleteInstance(WhatsAppInstanceDto instance)
    {
        var confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", $"¿Estás seguro de que quieres eliminar la instancia '{instance.Name}'?");
        if (!confirmed) return;

        try
        {
            var success = await _apiService.DeleteInstanceAsync(instance.Id);
            if (success)
            {
                await LoadInstancesAsync();
                _notification.ShowSuccess($"Instancia '{instance.Name}' eliminada exitosamente");
            }
            else
            {
                _notification.ShowError("Error al eliminar la instancia");
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error inesperado: {ex.Message}");
        }
    }
    private async Task ShowQrCode(WhatsAppInstanceDto instance)
    {
        try
        {
            var qrResponse = await _apiService.GetQrCodeAsync(instance.Id);
            if (qrResponse != null)
            {
                qrCodeImage = qrResponse.QrCode;
                showQrModal = true;
                StateHasChanged();
                _notification.ShowInfo("Escanea el código QR con WhatsApp para conectar la instancia");
            }
            else
            {
                _notification.ShowError("No se pudo obtener el código QR");
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error al obtener código QR: {ex.Message}");
        }
    }

    private void CloseQrModal()
    {
        showQrModal = false;
        qrCodeImage = null;
        StateHasChanged();
    }
    private void ManageInstance(WhatsAppInstanceDto instance)
    {
        // TODO: Navegar a página de administración de instancia
        _notification.ShowInfo($"Funcionalidad de administración para '{instance.Name}' próximamente disponible");
    }
}
