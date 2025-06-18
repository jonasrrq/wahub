using Microsoft.AspNetCore.Http;

namespace WaHub.Services;

public interface IThemeService
{
    string GetCurrentTheme();
    void SetTheme(string theme);
}

public class ThemeService : IThemeService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string THEME_COOKIE_NAME = "wahub-theme";
    private const string DEFAULT_THEME = "light";

    public ThemeService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentTheme()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request.Cookies.TryGetValue(THEME_COOKIE_NAME, out var theme) == true)
        {
            return string.IsNullOrEmpty(theme) ? DEFAULT_THEME : theme;
        }
        
        // Si no hay cookie, detectar preferencia del sistema desde headers
        var userAgent = httpContext?.Request.Headers["User-Agent"].ToString() ?? "";
        // Por ahora devolver default, se puede mejorar para detectar preferencia del sistema
        return DEFAULT_THEME;
    }

    public void SetTheme(string theme)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false, // Permitir acceso desde JavaScript
                Secure = httpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            };
            
            httpContext.Response.Cookies.Append(THEME_COOKIE_NAME, theme, cookieOptions);
        }
    }
}
