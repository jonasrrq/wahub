using System.ComponentModel.DataAnnotations;

using Microsoft.JSInterop;


namespace WaHub.Client.Pages;

public partial class Profile
{
    private readonly IJSRuntime _jSRuntime;

    public Profile(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    private string activeTab = "personal";
    private bool isSaving = false;

    private UserProfileModel userProfile = new()
    {
        FirstName = "Juan",
        LastName = "Pérez",
        Email = "juan.perez@example.com",
        Phone = "+1 (555) 123-4567",
        Company = "TechCorp Inc.",
        Bio = "Desarrollador Full Stack con experiencia en automatización de WhatsApp",
        Plan = "Professional",
        MonthlyPrice = "49.99",
        NextBilling = "15 de Enero, 2024",
        LastFourDigits = "1234",
        TwoFactorEnabled = false,
        Theme = "light",
        Language = "es",
        EmailNotifications = true,
        PushNotifications = true,
        Newsletter = false
    };

    private PasswordChangeModel passwordForm = new();

    private List<ActiveSession> activeSessions = new()
    {
        new() { Id = "1", DeviceName = "Chrome en Windows", DeviceType = "desktop", Location = "Madrid, España", LastActive = "Ahora", IsCurrent = true },
        new() { Id = "2", DeviceName = "Safari en iPhone", DeviceType = "mobile-alt", Location = "Madrid, España", LastActive = "Hace 2 horas", IsCurrent = false },
        new() { Id = "3", DeviceName = "Firefox en Ubuntu", DeviceType = "desktop", Location = "Barcelona, España", LastActive = "Ayer", IsCurrent = false }
    };

    private List<BillingInvoice> billingHistory = new()
    {
        new() { Date = "01 Dic 2023", Description = "Plan Professional - Diciembre", Amount = "49.99" },
        new() { Date = "01 Nov 2023", Description = "Plan Professional - Noviembre", Amount = "49.99" },
        new() { Date = "01 Oct 2023", Description = "Plan Professional - Octubre", Amount = "49.99" }
    };

    protected override async Task OnInitializedAsync()
    {
        // Aquí se cargaría el perfil del usuario desde el backend
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
            await _jSRuntime.InvokeVoidAsync("alert", "Información personal actualizada exitosamente");
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
            await _jSRuntime.InvokeVoidAsync("alert", "Contraseña cambiada exitosamente");
            passwordForm = new(); // Limpiar formulario
        }
        catch (Exception)
        {
            await _jSRuntime.InvokeVoidAsync("alert", "Error al cambiar la contraseña");
        }
    }

    private async Task ToggleTwoFactor()
    {
        userProfile.TwoFactorEnabled = !userProfile.TwoFactorEnabled;
        await _jSRuntime.InvokeVoidAsync("alert",
            $"Autenticación de dos factores {(userProfile.TwoFactorEnabled ? "activada" : "desactivada")}");
    }

    private async Task RevokeSession(string sessionId)
    {
        activeSessions.RemoveAll(s => s.Id == sessionId);
        await _jSRuntime.InvokeVoidAsync("alert", "Sesión cerrada exitosamente");
        StateHasChanged();
    }

    private async Task SavePreferences()
    {
        try
        {
            await Task.Delay(1000); // Simular llamada a API
            await _jSRuntime.InvokeVoidAsync("alert", "Preferencias guardadas exitosamente");
        }
        catch (Exception)
        {
            await _jSRuntime.InvokeVoidAsync("alert", "Error al guardar preferencias");
        }
    }

    private async Task ChangeAvatar()
    {
        await _jSRuntime.InvokeVoidAsync("alert", "Función de cambio de avatar próximamente disponible");
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
