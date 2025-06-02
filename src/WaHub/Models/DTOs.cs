namespace WaHub.Models
{
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

    public class CreateInstanceRequest
    {
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

    public class QrCodeResponse
    {
        public string QrCode { get; set; } = "";
        public string Status { get; set; } = "";
    }

    public class MessageDto
    {
        public string Id { get; set; } = "";
        public string InstanceId { get; set; } = "";
        public string InstanceName { get; set; } = "";
        public string RecipientPhone { get; set; } = "";
        public string RecipientName { get; set; } = "";
        public string Content { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public string MediaUrl { get; set; } = "";
    }

    public class NewMessageDto
    {
        public string InstanceId { get; set; } = "";
        public string RecipientPhone { get; set; } = "";
        public string Content { get; set; } = "";
        public string MediaUrl { get; set; } = "";
        public bool IsScheduled { get; set; } = false;
        public DateTime? ScheduledAt { get; set; }
    }

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

    // DTO para información del usuario autenticado
    public class UserInfo
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Avatar { get; set; } = "";
    }
}
