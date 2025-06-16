using System.ComponentModel.DataAnnotations;

using Microsoft.JSInterop;
using WaHub.Shared.Services;
using WaHub.Shared.Models.Resouces;

namespace WaHub.Client.Pages;

public partial class Profile
{
    private readonly IJSRuntime _jSRuntime;
    private readonly ILocalizationService _localization;

    public Profile(IJSRuntime jSRuntime, ILocalizationService localization)
    {
        _jSRuntime = jSRuntime;
        _localization = localization;
    }

    private string activeTab = "personal";
    private bool isSaving = false;

    private UserProfileModel userProfile = new();

    private PasswordChangeModel passwordForm = new();

    private List<ActiveSession> activeSessions = new();

    private List<BillingInvoice> billingHistory = new();

    protected override async Task OnInitializedAsync()
    {
        // Aquí se cargaría el perfil del usuario desde el backend
        userProfile = new()
        {
            FirstName = "Juan",
            LastName = "Pérez",
            Email = "juan.perez@example.com",
            Phone = "+1 (555) 123-4567",
            Company = "TechCorp Inc.",
            Bio = _localization.GetString(ResourceKeys.Profile_DefaultBio),
            Plan = "Professional",
            MonthlyPrice = "49.99",
            NextBilling = _localization.GetString(ResourceKeys.Profile_NextBilling_Date),
            LastFourDigits = "1234",
            TwoFactorEnabled = false,
            Theme = "light",
            Language = "es",
            EmailNotifications = true,
            PushNotifications = true,
            Newsletter = false
        };

        activeSessions = new()
        {
            new() { Id = "1", DeviceName = "Chrome en Windows", DeviceType = "desktop", Location = _localization.GetString(ResourceKeys.Profile_Session_Location_Madrid), LastActive = _localization.GetString(ResourceKeys.Profile_Session_LastActive_Now), IsCurrent = true },
            new() { Id = "2", DeviceName = "Safari en iPhone", DeviceType = "mobile-alt", Location = _localization.GetString(ResourceKeys.Profile_Session_Location_Madrid), LastActive = _localization.GetString(ResourceKeys.Profile_Session_LastActive_2HoursAgo), IsCurrent = false },
            new() { Id = "3", DeviceName = "Firefox en Ubuntu", DeviceType = "desktop", Location = _localization.GetString(ResourceKeys.Profile_Session_Location_Barcelona), LastActive = _localization.GetString(ResourceKeys.Profile_Session_LastActive_Yesterday), IsCurrent = false }
        };

        billingHistory = new()
        {
            new() { Date = "01 Dic 2023", Description = _localization.GetString(ResourceKeys.Profile_Billing_ProfessionalPlan_Dec), Amount = "49.99" },
            new() { Date = "01 Nov 2023", Description = _localization.GetString(ResourceKeys.Profile_Billing_ProfessionalPlan_Nov), Amount = "49.99" },
            new() { Date = "01 Oct 2023", Description = _localization.GetString(ResourceKeys.Profile_Billing_ProfessionalPlan_Oct), Amount = "49.99" }
        };

        await Task.CompletedTask;
    }

    private void SetActiveTab(string tab)
    {
        activeTab = tab;
    }

    private async Task SavePersonalInfo()
    {
        isSaving = true;
        StateHasChanged();

        try
        {
            await Task.Delay(1000); // Simular llamada a API
            await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.PersonalInfoUpdatedSuccessfully));
        }
        finally
        {
            isSaving = false;
            StateHasChanged();
        }
    }

    private async Task ChangePassword()
    {
        try
        {
            await Task.Delay(1000); // Simular llamada a API
            await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.PasswordChangedSuccessfully));
            passwordForm = new(); // Limpiar formulario
        }
        catch (Exception ex)
        {
            await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.ErrorChangingPassword, ex.Message));
        }
    }

    private async Task ToggleTwoFactor()
    {
        userProfile.TwoFactorEnabled = !userProfile.TwoFactorEnabled;
        await _jSRuntime.InvokeVoidAsync("alert",
            _localization.GetString(
                userProfile.TwoFactorEnabled ? ResourceKeys.TwoFactorEnabled : ResourceKeys.TwoFactorDisabled));
    }

    private async Task RevokeSession(string sessionId)
    {
        activeSessions.RemoveAll(s => s.Id == sessionId);
        await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.SessionClosedSuccessfully));
        StateHasChanged();
    }

    private async Task SavePreferences()
    {
        try
        {
            await Task.Delay(1000); // Simular llamada a API
            await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.PreferencesSavedSuccessfully));
        }
        catch (Exception)
        {
            await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.ErrorSavingPreferences));
        }
    }

    private async Task ChangeAvatar()
    {
        await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.ChangeAvatarComingSoon));
    }

    private async Task DeleteAccountConfirm()
    {
        var confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", _localization.GetString(ResourceKeys.DeleteAccountConfirm));
        if (confirmed)
        {
            try
            {
                // Lógica para eliminar la cuenta
                await Task.Delay(1000); // Simular operación asíncrona
                await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.DeleteAccountSuccess));
                // Redirigir o cerrar sesión
            }
            catch (Exception ex)
            {
                await _jSRuntime.InvokeVoidAsync("alert", _localization.GetString(ResourceKeys.DeleteAccountError, ex.Message));
            }
        }
    }

    public class UserProfileModel
    {
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required][EmailAddress] public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Company { get; set; } = "";
        public string Bio { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public string Plan { get; set; } = "";
        public string MonthlyPrice { get; set; } = "";
        public string NextBilling { get; set; } = "";
        public string LastFourDigits { get; set; } = "";
        public bool TwoFactorEnabled { get; set; }
        public string Theme { get; set; } = "";
        public string Language { get; set; } = "";
        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        public bool Newsletter { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class PasswordChangeModel
    {
        [Required] public string CurrentPassword { get; set; } = "";
        [Required][MinLength(8)] public string NewPassword { get; set; } = "";
        [Required][Compare(nameof(NewPassword))] public string ConfirmPassword { get; set; } = "";
    }

    public class ActiveSession
    {
        public string Id { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string Location { get; set; } = "";
        public string LastActive { get; set; } = "";
        public bool IsCurrent { get; set; }
    }

    public class BillingInvoice
    {
        public string Date { get; set; } = "";
        public string Description { get; set; } = "";
        public string Amount { get; set; } = "";
    }
}
