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

        if (parameter is null)
        {
            parameter = "assessments";
        }

        IEnumerable<Assessments>? assessments = null;
        int total = 0;
        try
        {
            var response = await HttpClient.GetAsync($"/v1/{parameter}");
            (total, assessments) = await HandlerApiResponseAsync<IEnumerable<Assessments>>(response, null);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return (total, assessments);
    }
    public async Task<Assessments> GetAssessmentByName(string query)
    {
        Assessments? assessments = null;
        int total = 0;
        try
        {
            var response = await HttpClient.GetAsync($"/{query}");

            using var responseStream = await response.Content.ReadAsStreamAsync();
            (total, assessments) = await HandlerApiResponseAsync<Assessments>(response, null);

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessments;
    }

    public async Task<Assessments> GetAssessmentById(int id)
    {
        Assessments? assessments = null;
        int total = 0;
        try
        {
            var response = await HttpClient.GetAsync($"/assessments/{id}");
            (total, assessments) = await HandlerApiResponseAsync<Assessments>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessments;
    }
    public async Task<Assessments> PostAssessment(Assessments assessment)
    {
        Assessments? assessmentUpdate = null;
        int total = 0;
        try
        {
            string jsonRespponse = JsonSerializer.Serialize<object>(assessment, JsonSerializerOptions);
            StringContent content = new(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("/assessment", content);
            (total, assessmentUpdate) = await HandlerApiResponseAsync<Assessments>(response, null);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessmentUpdate;
    }
    //public async Task<Assessments> PutAssessment(int id, Assessments assessment)
    //{
    //    Assessments? assessmentUpdate = null;
    //    try
    //    {
    //        string jsonRespponse = JsonSerializer.Serialize<object>(assessment, _jsonOptions);

    //        StringContent content = new StringContent(jsonRespponse, Encoding.UTF8, "application/json");

    //        var response = await _client.PutAsync($"{Configurations.Url}/assessment/{id}", content);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            using var responseStream = await response.Content.ReadAsStreamAsync();
    //            assessmentUpdate = await JsonSerializer.DeserializeAsync<Assessments>(responseStream, _jsonOptions);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ex.ToString();
    //    }

    //    return assessmentUpdate;
    //}
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
