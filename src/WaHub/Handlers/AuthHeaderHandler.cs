using System.Net.Http.Headers;
using WaHub.Services;

namespace WaHub.Blazor.Handlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ICrossPlatformStorageService _storageService;
    private readonly ILogger<AuthHeaderHandler> _logger;

    public AuthHeaderHandler(ICrossPlatformStorageService storageService, ILogger<AuthHeaderHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var tokenResult = await _storageService.GetAsync<string>("token");
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
