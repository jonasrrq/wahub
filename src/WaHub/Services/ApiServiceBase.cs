using WaHub.Shared.Models;


namespace WaHub.Services;

public abstract class ApiServiceBase
{
    private readonly HttpClient _httpClient;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory, string instance)
    {
        _httpClient = httpClientFactory.CreateClient(instance);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        return await _httpClient.DeleteAsync(url);       
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        return await _httpClient.GetFromJsonAsync<T>(url);
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
    {
        return await _httpClient.PostAsJsonAsync(url, data);
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
    {
        return await _httpClient.PutAsJsonAsync(url, data);
    }
}