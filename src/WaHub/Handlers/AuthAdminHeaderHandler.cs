using System.Net.Http.Headers;

namespace WaHub.Handlers;

public class AuthAdminHeaderHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthAdminHeaderHandler> _logger;

    public AuthAdminHeaderHandler(
        IConfiguration configuration,
        ILogger<AuthAdminHeaderHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var admintoken = _configuration.GetValue<string>("TokenAdmin");
            //request.RequestUri = new Uri(_configuration.GetValue<string>("UriApi") ?? "", UriKind.Absolute);         
            request.Headers.Add("Authorization", admintoken);
            if (request.Content != null)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            //request.Headers.Add("Content-Type", "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error adding auth header");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
