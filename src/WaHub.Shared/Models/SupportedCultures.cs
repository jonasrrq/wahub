using System.Globalization;

namespace WaHub.Shared.Models;

public static class SupportedCultures
{
    public static readonly CultureInfo[] All = new[]
    {
        new CultureInfo("es"),
        new CultureInfo("en"),
        new CultureInfo("fr"),
        new CultureInfo("pt"),
        new CultureInfo("pt-BR")
    };

    public const string Default = "es";
}
