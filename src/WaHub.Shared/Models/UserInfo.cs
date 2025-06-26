namespace WaHub.Shared.Models;


// DTO para información del usuario autenticado
public class UserInfo
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = ""; // Mantener para compatibilidad hacia atrás
    public IList<string> Roles { get; set; } = new List<string>(); // Soporte para múltiples roles
    public string Avatar { get; set; } = "";
}
