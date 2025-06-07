using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class ApiAdminService : ApiServiceBase, IApiAdminService
{
    public ApiAdminService(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "ApiAdminHttpClient") { }

    public async Task<Response<List<InstanceDto>>> GetUsersAsync()
    {
        try
        {
            return await GetAsync<Response<List<InstanceDto>>>("/admin/users") ?? new Response<List<InstanceDto>>();
        }
        catch
        {
            return new Response<List<InstanceDto>>();
        }
    }

    public async Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request)
    {
        var result = await PostAsync("/admin/users", request);

        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<Response<InstanceDto>>() ?? new Response<InstanceDto>();
        }
        else
        {
            var response = new Response<InstanceDto>();
            response.Success = false;
            response.Message = "Error al crear la instancia. Por favor, inténtalo de nuevo más tarde.";
            return response;
        }
    }
}
