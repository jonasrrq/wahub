using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace WaHub.Blazor.Handlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ILogger<AuthHeaderHandler> _logger;

    public AuthHeaderHandler(ProtectedLocalStorage localStorage, ILogger<AuthHeaderHandler> logger)
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>("token");
            if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error adding auth header");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
