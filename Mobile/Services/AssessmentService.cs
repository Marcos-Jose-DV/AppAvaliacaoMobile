using DocumentFormat.OpenXml.VariantTypes;
using Mobile.Constans;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mobile.Services;

public class AssessmentService : BaseApiService
{

    public AssessmentService(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory) { }

    public async Task<(int total, IEnumerable<Assessments>)> GetAssessments(string parameter)
    {
        ApiResponse apiResponse = null;

        try
        {
            var response = await HttpClient.GetAsync($"/v1/{parameter}");
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }


        return (apiResponse.Total, apiResponse.Datas);

    }
    public async Task<(int total, IEnumerable<Assessments>)> GetLoadCategoryAsync(string parameter)
    {
        ApiResponse apiResponse = null;

        if (parameter is null)
        {
            parameter = "assessments";
        }

        try
        {
            var response = await HttpClient.GetAsync($"/v1/{parameter}");
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return (apiResponse.Total, apiResponse.Datas);
    }
    public async Task<Assessments> GetAssessmentByName(string query)
    {
        ApiResponse apiResponse = null;

        try
        {
            var response = await HttpClient.GetAsync($"/v1/{query}");

            using var responseStream = await response.Content.ReadAsStreamAsync();
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return apiResponse.Data;
    }

    public async Task<Assessments> GetAssessmentById(int id)
    {
        ApiResponse apiResponse = null;

        try
        {
            var response = await HttpClient.GetAsync($"/v1/assessments/{id}");
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return apiResponse.Data;
    }
    public async Task<Assessments> PostAssessment(Assessments assessmentPost)
    {
        ApiResponse apiResponse = null;
        try
        {
            string jsonRespponse = JsonSerializer.Serialize<object>(assessmentPost, JsonSerializerOptions);
            StringContent content = new(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("/v1/assessment", content);
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return apiResponse.Data;
    }
    public async Task<string> PostAssessments(List<Assessments> assessmentPost)
    {
        ApiResponse apiResponse = null;
        try
        {
            string jsonRespponse = JsonSerializer.Serialize<object>(assessmentPost, JsonSerializerOptions);
            StringContent content = new(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("/v1/assessments", content);
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
            return "No";
        }

        return "OK";
    }
    public async Task<Assessments> PutAssessment(int id, Assessments assessment)
    {
        ApiResponse apiResponse = null;
        try
        {
            string jsonRespponse = JsonSerializer.Serialize<object>(assessment, JsonSerializerOptions);
            StringContent content = new(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await HttpClient.PutAsync($"/v1/assessment/{id}", content);
            apiResponse = await HandlerApiResponseAsync<ApiResponse>(response, null);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return apiResponse.Data;
    }
    //public async Task<Assessments> DeleteAssessment(int id)
    //{
    //    Assessments? assessment = null;
    //    try
    //    {
    //        var response = await _client.DeleteAsync($"{Configurations.Url}/assessment/{id}");
    //        if (response.IsSuccessStatusCode)
    //        {
    //            using var responseStream = await response.Content.ReadAsStreamAsync();
    //            assessment = await JsonSerializer.DeserializeAsync<Assessments>(responseStream, _jsonOptions);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ex.ToString();
    //    }

    //    return assessment;
    //}
}
