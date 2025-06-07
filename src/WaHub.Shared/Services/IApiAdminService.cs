using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances;

namespace WaHub.Shared.Services;

public interface IApiAdminService
{
    Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request);
    Task<Response<List<InstanceDto>>> GetUsersAsync();
}