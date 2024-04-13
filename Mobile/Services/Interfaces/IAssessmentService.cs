
using Mobile.Models;

namespace Mobile.Services.Interfaces;

public interface IAssessmentService
{
    Task<IEnumerable<Assessments>> GetAssessments(string parameter);
    Task<Assessments> GetAssessmentById(int id);
    Task<Assessments> GetAssessmentByName(string query);
    Task<Assessments> PostAssessment(Assessments assessment);
    Task<Assessments> PutAssessment(int id, Assessments assessment);
    Task<Assessments> DeleteAssessment(int id);
}
