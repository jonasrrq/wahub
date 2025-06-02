namespace WaHub.Shared.Models;

// Modelo interno para compatibilidad
public class WhatsAppInstance
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public bool IsActive { get; set; }
    public string Status { get; set; } = "";
    public int MessagesSent { get; set; }
    public int Contacts { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastActivity { get; set; }
}
