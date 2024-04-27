using ClosedXML.Excel;
using Models.Models;

namespace Herpels.FileHelpers;

public static class ReadFile
{
    public async static Task<IEnumerable<Assessments>> ReadExcel()
    {
        var result = await PickerFile.PickerFileAsync();
        using var fileStream = new FileStream(result.FullPath, FileMode.Open);

        using var workbook = new XLWorkbook(fileStream);        
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
        if (assessments.Count == 0)
            throw new Exception("O arquivo carregado estar vazio.");
        return assessments;
    }
}
