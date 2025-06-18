using Microsoft.AspNetCore.Mvc;
using WaHub.Services;

namespace WaHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThemeController : ControllerBase
{
    private readonly IThemeService _themeService;

    public ThemeController(IThemeService themeService)
    {
        _themeService = themeService;
    }

    [HttpPost("set")]
    public IActionResult SetTheme([FromBody] SetThemeRequest request)
    {
        if (string.IsNullOrEmpty(request.Theme) || 
            (request.Theme != "light" && request.Theme != "dark"))
        {
            return BadRequest("Invalid theme. Must be 'light' or 'dark'.");
        }

        _themeService.SetTheme(request.Theme);
        return Ok(new { theme = request.Theme });
    }

    [HttpGet("current")]
    public IActionResult GetCurrentTheme()
    {
        var currentTheme = _themeService.GetCurrentTheme();
        return Ok(new { theme = currentTheme });
    }
}

public class SetThemeRequest
{
    public string Theme { get; set; } = string.Empty;
}
