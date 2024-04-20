using Mobile.Constans;
using Mobile.Models;
using System.Text.Json;

namespace Mobile.Services;

public class BaseApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseApiService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    protected JsonSerializerOptions JsonSerializerOptions
      = new() { PropertyNameCaseInsensitive = true };

    protected HttpClient HttpClient
        => _httpClientFactory.CreateClient(Configurations.HttpClientName);

    protected TData DeserializeApiResponse<TData>(string jsondata)
        => JsonSerializer.Deserialize<TData>(jsondata, JsonSerializerOptions);

    protected async Task<ApiResponse> HandlerApiResponseAsync<TData>(HttpResponseMessage response, ApiResponse defaultValue)
    {
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                var apiResponse = DeserializeApiResponse<ApiResponse>(content);
                return apiResponse;
            }
        }

        return defaultValue;
    }

}
