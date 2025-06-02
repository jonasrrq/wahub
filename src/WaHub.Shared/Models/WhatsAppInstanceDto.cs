namespace WaHub.Shared.Models;

// DTOs para WhatsApp API
public class WhatsAppInstanceDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Status { get; set; } = "";
    public bool IsActive { get; set; }
    public int MessagesSent { get; set; }
    public int Contacts { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastActivity { get; set; }
}
