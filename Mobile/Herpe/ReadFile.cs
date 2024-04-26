using ClosedXML.Excel;
using Models.Models;
using Models.Models.ViewModels;

namespace Mobile.Herpels;

public static class ReadFile
{
    public static List<Assessments> ReadExcel(string path)
    {
        using var workbook = new XLWorkbook(path);
        var worksheet = workbook.Worksheet(1);
        var colMax = worksheet.ColumnsUsed().Count();
        var rowMax = worksheet.RowsUsed().Count();

        var assessments = new List<Assessments>();
        
        for (int row = 2; row <= rowMax; row++)
        {
            var assessment = new Assessments
            {
                Name = worksheet.Cell(row, 1).Value.ToString(),
                Assessment = worksheet.Cell(row, 2).Value.ToString(),
                Category = worksheet.Cell(row, 3).Value.ToString(),
                Director = worksheet.Cell(row, 4).Value.ToString(),
                Image = worksheet.Cell(row, 5).Value.ToString(),
                Gender = worksheet.Cell(row, 6).Value.ToString(),
                Duration = int.Parse(worksheet.Cell(row, 7).Value.ToString()),
                Concluded = bool.Parse(worksheet.Cell(row, 8).Value.ToString())
            };
            assessments.Add(assessment);
        }

        return assessments;
    }
}
