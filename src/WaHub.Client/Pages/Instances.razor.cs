using Microsoft.JSInterop;

using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances;
using WaHub.Shared.Services;


namespace WaHub.Client.Pages;

public partial class Instances
{
    private readonly IApiAdminService _apiService;
    private readonly NotificationService _notification;
    private readonly ILocalizationService _localization;
    private readonly IJSRuntime _jSRuntime;
    private List<InstanceDto> instances = new();
    private bool isLoading = true;
    private string? errorMessage;
    private bool showCreateModal = false;
    private bool showQrModal = false;
    private string createInstanceName = "";
    private string createInstancePhone = "";
    private string? qrCodeImage;


    public Instances(IApiAdminService apiService,
        NotificationService notification,
        ILocalizationService localization,
        IJSRuntime jSRuntime)
    {
        _apiService = apiService;
        _notification = notification;
        _localization = localization;
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
            var result = await _apiService.GetUsersAsync();

            if (result == null || !result.Success)
            {
                errorMessage = result?.Message ?? "Error al cargar las instancias";
                _notification.ShowError(errorMessage);
                return;
            }

            // instances = result.Data ?? new List<InstanceDto>(); // Original line - result.Data is List<UserDto>, instances is List<InstanceDto>
            instances = new List<InstanceDto>(); // Temporary fix for build. Data source is incorrect.
            _notification.ShowError("Data for instances is not being loaded correctly. Please check Instances.razor.cs.");
            // TODO: Create and use an appropriate method in IApiAdminService to fetch InstanceDto list.
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
            var request = new InstanceDto
            {
                Name = createInstanceName,
                Token = ""
            };

            var success = await _apiService.CreateInstanceAsync(request);
            if (success.Success)
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
    private async Task ToggleInstance(InstanceDto instance)
    {
        try
        {
            var status = instance.Connected ? "pausada" : "activada";
            //var success = await _apiService.ToggleInstanceAsync(instance.Id);
            //if (success)
            //{
            //    await LoadInstancesAsync();
            //    _notification.ShowSuccess($"Instancia '{instance.Name}' {status} correctamente");
            //}
            //else
            //{
            //    _notification.ShowError("Error al cambiar el estado de la instancia");
            //}
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error inesperado: {ex.Message}");
        }
    }
    private async Task DeleteInstance(InstanceDto instance)
    {
        var confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", $"¿Estás seguro de que quieres eliminar la instancia '{instance.Name}'?");
        if (!confirmed) return;

        try
        {
            //var success = await _apiService.DeleteInstanceAsync(instance.Id);
            //if (success)
            //{
            //    await LoadInstancesAsync();
            //    _notification.ShowSuccess($"Instancia '{instance.Name}' eliminada exitosamente");
            //}
            //else
            //{
            //    _notification.ShowError("Error al eliminar la instancia");
            //}
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error inesperado: {ex.Message}");
        }
    }
    private async Task ShowQrCode(InstanceDto instance)
    {
        try
        {
            //var qrResponse = await _apiService.GetQrCodeAsync(instance.Id);
            //if (qrResponse != null)
            //{
            //    qrCodeImage = qrResponse.QrCode;
            //    showQrModal = true;
            //    StateHasChanged();
            //    _notification.ShowInfo("Escanea el código QR con WhatsApp para conectar la instancia");
            //}
            //else
            //{
            //    _notification.ShowError("No se pudo obtener el código QR");
            //}
            await Task.CompletedTask;
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
    private void ManageInstance(InstanceDto instance)
    {
        // TODO: Navegar a página de administración de instancia
        _notification.ShowInfo($"Funcionalidad de administración para '{instance.Name}' próximamente disponible");
    }
}
