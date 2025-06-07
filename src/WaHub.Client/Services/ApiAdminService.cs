
using System.Net.Http.Json;

using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances;
using WaHub.Shared.Services;

namespace WaHub.Client.Services;

//internal class ApiAdminService : IApiAdminService
//{
    //private readonly HttpClient _httpClient;

    //public ApiAdminService(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    //public async Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request)
    //{
    //    return await _httpClient.GetFromJsonAsync<Response<InstanceDto>>("api/UserAdmin/users") 
    //        ?? new Response<InstanceDto>();
    //}

    //public Task<Response<List<InstanceDto>>> GetUsersAsync()
    //{
    //    throw new NotImplementedException();
    //}
//}
