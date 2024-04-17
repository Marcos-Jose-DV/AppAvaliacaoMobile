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

    protected async Task<(int Total, TData Data)> HandlerApiResponseAsync<TData>(HttpResponseMessage response, TData defaultValue)
    {
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                ApiResponse<TData> assessments = DeserializeApiResponse<ApiResponse<TData>>(content);

                return (assessments.Total, assessments.Data);
            }
        }

        return (0, defaultValue);
    }

    public class ApiResponse<TData>
    {
        public int Total { get; set; }
        public TData Data { get; set; }
    }
}
