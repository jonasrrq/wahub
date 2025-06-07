using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class ApiService : ApiServiceBase, IApiService
{
    //private readonly HttpClient _httpClient;

    public ApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "ApiHttpClient") { }
    //{
    //    //_httpClient = httpClientFactory.CreateClient("AuthHttpClient");
    //}

    // Métodos específicos para WhatsApp API
    public async Task<List<WhatsAppInstanceDto>> GetInstancesAsync()
    {
        try
        {
            var instances = await GetAsync<WhatsAppInstance[]>("/api/instances");
            return instances?.Select(i => new WhatsAppInstanceDto
            {
                Id = i.Id,
                Name = i.Name,
                PhoneNumber = i.PhoneNumber,
                Status = i.Status,
                IsActive = i.IsActive,
                MessagesSent = i.MessagesSent,
                Contacts = i.Contacts,
                CreatedAt = i.CreatedAt,
                LastActivity = i.LastActivity
            }).ToList() ?? new List<WhatsAppInstanceDto>();
        }
        catch
        {
            // Si falla la API, devolver datos simulados para desarrollo
            return GetMockInstances().Select(i => new WhatsAppInstanceDto
            {
                Id = i.Id,
                Name = i.Name,
                PhoneNumber = i.PhoneNumber,
                Status = i.Status,
                IsActive = i.IsActive,
                MessagesSent = i.MessagesSent,
                Contacts = i.Contacts,
                CreatedAt = i.CreatedAt,
                LastActivity = i.LastActivity
            }).ToList();
        }
    }

    public async Task<WhatsAppInstance[]?> GetWhatsAppInstancesAsync()
    {
        try
        {
            return await GetAsync<WhatsAppInstance[]>("/api/instances");
        }
        catch
        {
            // Si falla la API, devolver datos simulados para desarrollo
            return GetMockInstances();
        }
    }

    public async Task<WhatsAppInstance?> GetInstanceAsync(string instanceId)
    {
        try
        {
            return await GetAsync<WhatsAppInstance>($"/api/instances/{instanceId}");
        }
        catch
        {
            return GetMockInstances().FirstOrDefault(i => i.Id == instanceId);
        }
    }

    public async Task<bool> CreateInstanceAsync(CreateInstanceRequest request)
    {
        try
        {
            var response = await PostAsync("/api/instances", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteInstanceAsync(string instanceId)
    {
        try
        {
            var response = await DeleteAsync($"/api/instances/{instanceId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ToggleInstanceAsync(string instanceId)
    {
        try
        {
            var response = await PostAsync($"/api/instances/{instanceId}/toggle", new { });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<QrCodeResponse?> GetQrCodeAsync(string instanceId)
    {
        try
        {
            return await GetAsync<QrCodeResponse>($"/api/instances/{instanceId}/qr");
        }
        catch
        {
            // Simular QR code para desarrollo
            return new QrCodeResponse
            {
                QrCode = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg=="
            };
        }
    }

    // Métodos para gestión de mensajes
    public async Task<List<MessageDto>> GetMessagesAsync()
    {
        try
        {
            return await GetAsync<List<MessageDto>>("/api/messages") ?? new List<MessageDto>();
        }
        catch
        {
            // Si falla la API, devolver datos simulados para desarrollo
            return GetMockMessages();
        }
    }

    public async Task<MessageDto?> GetMessageAsync(string messageId)
    {
        try
        {
            return await GetAsync<MessageDto>($"/api/messages/{messageId}");
        }
        catch
        {
            return GetMockMessages().FirstOrDefault(m => m.Id == messageId);
        }
    }

    public async Task<bool> SendMessageAsync(NewMessageDto message)
    {
        try
        {
            var response = await PostAsync("/api/messages", message);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ResendMessageAsync(string messageId)
    {
        try
        {
            var response = await PostAsync($"/api/messages/{messageId}/resend", new { });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CancelScheduledMessageAsync(string messageId)
    {
        try
        {
            var response = await DeleteAsync($"/api/messages/{messageId}/cancel");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Datos simulados para desarrollo
    private WhatsAppInstance[] GetMockInstances()
    {
        return new[]
        {
            new WhatsAppInstance
            {
                Id = "instance-1",
                Name = "Instancia Principal",
                PhoneNumber = "+1234567890",
                IsActive = true,
                Status = "Conectado",
                MessagesSent = 1245,
                Contacts = 89,
                CreatedAt = DateTime.Now.AddDays(-15),
                LastActivity = DateTime.Now.AddMinutes(-5)
            },
            new WhatsAppInstance
            {
                Id = "instance-2",
                Name = "Instancia Secundaria",
                PhoneNumber = "+0987654321",
                IsActive = false,
                Status = "Desconectado",
                MessagesSent = 456,
                Contacts = 23,
                CreatedAt = DateTime.Now.AddDays(-7),
                LastActivity = DateTime.Now.AddHours(-2)
            }
        };
    }

    // Datos simulados para mensajes
    private List<MessageDto> GetMockMessages()
    {
        return new List<MessageDto>
        {
            new MessageDto
            {
                Id = "msg-1",
                InstanceId = "instance-1",
                InstanceName = "Instancia Principal",
                RecipientPhone = "+34666123456",
                RecipientName = "Juan Pérez",
                Content = "¡Hola! ¿Cómo estás hoy? Este es un mensaje de prueba desde WaHub.",
                Status = "sent",
                CreatedAt = DateTime.Now.AddHours(-2),
                SentAt = DateTime.Now.AddHours(-2)
            },
            new MessageDto
            {
                Id = "msg-2",
                InstanceId = "instance-1",
                InstanceName = "Instancia Principal",
                RecipientPhone = "+34666789012",
                RecipientName = "María García",
                Content = "Recordatorio: Su cita está programada para mañana a las 10:00 AM.",
                Status = "pending",
                CreatedAt = DateTime.Now.AddMinutes(-30)
            },
            new MessageDto
            {
                Id = "msg-3",
                InstanceId = "instance-2",
                InstanceName = "Instancia Secundaria",
                RecipientPhone = "+34666345678",
                Content = "Error en la entrega del mensaje anterior.",
                Status = "failed",
                CreatedAt = DateTime.Now.AddHours(-1)
            },
            new MessageDto
            {
                Id = "msg-4",
                InstanceId = "instance-1",
                InstanceName = "Instancia Principal",
                RecipientPhone = "+34666111222",
                RecipientName = "Carlos López",
                Content = "Mensaje programado para envío posterior.",
                Status = "scheduled",
                CreatedAt = DateTime.Now.AddMinutes(-15),
                ScheduledAt = DateTime.Now.AddHours(2)
            },
            new MessageDto
            {
                Id = "msg-5",
                InstanceId = "instance-1",
                InstanceName = "Instancia Principal",
                RecipientPhone = "+34666888999",
                RecipientName = "Ana Martín",
                Content = "Gracias por contactarnos. Hemos recibido su consulta y le responderemos a la brevedad.",
                MediaUrl = "https://example.com/file.pdf",
                Status = "sent",
                CreatedAt = DateTime.Now.AddDays(-1),
                SentAt = DateTime.Now.AddDays(-1)
            },
            new MessageDto
            {
                Id = "msg-6",
                InstanceId = "instance-2",
                InstanceName = "Instancia Secundaria",
                                    RecipientPhone = "+34666555444",
                Content = "Promoción especial: 20% de descuento en todos nuestros productos. ¡No te lo pierdas!",
                Status = "sent",
                CreatedAt = DateTime.Now.AddDays(-2),
                SentAt = DateTime.Now.AddDays(-2)
            }
        };
    }
}
