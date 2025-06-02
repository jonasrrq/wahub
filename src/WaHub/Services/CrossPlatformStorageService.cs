using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace WaHub.Services;

public interface ICrossPlatformStorageService
{
    Task SetAsync<T>(string key, T value);
    Task<(bool Success, T? Value)> GetAsync<T>(string key);
    Task DeleteAsync(string key);
}

public class CrossPlatformStorageService : ICrossPlatformStorageService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly IJSRuntime _jsRuntime;

    public CrossPlatformStorageService(ProtectedLocalStorage protectedLocalStorage, IJSRuntime jsRuntime)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _jsRuntime = jsRuntime;
    }

    public async Task SetAsync<T>(string key, T value)
    {
        if (OperatingSystem.IsBrowser())
        {
            // WebAssembly: usar JS interop
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
        else
        {
            // Server: usar ProtectedLocalStorage
            await _protectedLocalStorage.SetAsync(key, value);
        }
    }

    public async Task<(bool Success, T? Value)> GetAsync<T>(string key)
    {
        if (OperatingSystem.IsBrowser())
        {
            // WebAssembly: usar JS interop
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var value = JsonSerializer.Deserialize<T>(json);
                    return (true, value);
                }
                catch { }
            }
            return (false, default);
        }
        else
        {
            // Server: usar ProtectedLocalStorage
            var result = await _protectedLocalStorage.GetAsync<T>(key);
            return (result.Success, result.Success ? result.Value : default);
        }
    }

    public async Task DeleteAsync(string key)
    {
        if (OperatingSystem.IsBrowser())
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        else
        {
            await _protectedLocalStorage.DeleteAsync(key);
        }
    }
}
