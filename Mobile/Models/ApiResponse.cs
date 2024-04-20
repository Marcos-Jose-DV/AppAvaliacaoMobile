namespace Mobile.Models;

public class ApiResponse
{
    public int Total { get; set; }
    public IEnumerable<Assessments> Datas { get; set; }
    public Assessments Data { get; set; }
    public string Uri { get; set; }
    public string Value { get; set; }
}