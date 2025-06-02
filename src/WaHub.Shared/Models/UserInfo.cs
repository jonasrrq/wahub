namespace WaHub.Shared.Models;


// DTO para información del usuario autenticado
public class UserInfo
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string Avatar { get; set; } = "";
}
