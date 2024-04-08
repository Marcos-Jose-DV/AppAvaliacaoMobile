using Mobile.Constans;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mobile.Services;

public class AssessmentService : IAssessmentService
{
    HttpClient _client;
    JsonSerializerOptions _jsonOptions;

    public AssessmentService()
    {
        _client = new HttpClient();
        _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
 
   


    public async Task<IEnumerable<Assessments>> GetAssessments()
    {
        IEnumerable<Assessments>? assessments = null;
        try
        {
            var response = await _client.GetAsync($"{Configurations.Url}/assessments");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                assessments = await JsonSerializer.DeserializeAsync<IEnumerable<Assessments>>(responseStream, _jsonOptions);
            }

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
        try
        {
            var response = await _client.GetAsync($"{Configurations.Url}/assessments/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                assessments = await JsonSerializer.DeserializeAsync<Assessments>(responseStream, _jsonOptions);
            }

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessments;
    }

    public Task<Assessments> PostAssessment(Assessments movie)
    {
        throw new NotImplementedException();
    }

    public async Task<Assessments> PutAssessment(int id, Assessments assessment)
    {
        Assessments? assessmentUpdate = null;
        try
        {

            string jsonRespponse = JsonSerializer.Serialize<object>(assessment, _jsonOptions);

            StringContent content = new StringContent(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{Configurations.Url}/assessment/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                assessmentUpdate = await JsonSerializer.DeserializeAsync<Assessments>(responseStream, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessmentUpdate;
    }

    public async Task<Assessments> DeleteAssessment(int id)
    {
        Assessments? assessment = null;
        try
        {
            var response = await _client.DeleteAsync($"{Configurations.Url}/assessment/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                assessment = await JsonSerializer.DeserializeAsync<Assessments>(responseStream, _jsonOptions);
            }

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return assessment;
    }
}
