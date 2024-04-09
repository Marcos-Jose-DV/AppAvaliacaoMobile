using ClosedXML.Excel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTextSharp.text.pdf;
using Mobile.Models;
using Mobile.Services.Interfaces;
using Xceed.Words.NET;
using Document = iTextSharp.text.Document;

namespace Mobile.ViewModels;

public partial class DownloadViewModel : ObservableObject
{

    private readonly IFileSaver _fileSaver;
    private readonly IAssessmentService _assessmentService;

    public DownloadViewModel(IFileSaver fileSaver, IAssessmentService assessmentService)
    {
        _fileSaver = fileSaver;
        _assessmentService = assessmentService;
    }

    [RelayCommand]
    async Task SaveFile(string fileName)
    {
        var cancellationToken = new CancellationToken();
        try
        {
            var assessments = await _assessmentService.GetAssessments();
            Stream stream = null;

            if (fileName.Equals("assessments.txt"))
            {
                stream = WriteTxt(assessments);
            }
            else if (fileName.Equals("assessments.pdf"))
            {
                stream = WritePDF(assessments);
            }
            else if (fileName.Equals("assessments.xlsx"))
            {
                stream = WriteExcel(assessments);
            }
            else
            {
                stream = WriteDoxc(assessments);
            }

            var fileLocationResult = await _fileSaver.SaveAsync(fileName, stream, cancellationToken);
            fileLocationResult.EnsureSuccess();
            await stream.DisposeAsync();
            await Toast.Make($"Arquivo salvo em: {fileLocationResult.FilePath}").Show(cancellationToken);
        }
        catch (Exception ex)
        {
            await Toast.Make($"O arquivo não foi salvo: {ex.Message}").Show(cancellationToken);
        }
    }

    private Stream? WriteDoxc(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        using DocX docx = DocX.Create(stream);
        Xceed.Document.NET.Paragraph paragraph = docx.InsertParagraph();

        foreach (var assessment in assessments)
        {

            paragraph.Append($"Id: {assessment.Id}, Title: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");

            paragraph.AppendLine();
        }
        docx.Save();
        var array = new MemoryStream(stream.ToArray());
        return array;
    }

    private Stream WriteTxt(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        using StreamWriter writer = new(stream);
        foreach (var assessment in assessments)
        {
            writer.WriteLine($"Id: {assessment.Id}, Title: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");
        }

        var array = new MemoryStream(stream.ToArray());
        return array;
    }

    private Stream WritePDF(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        Document doc = new();
        PdfWriter.GetInstance(doc, stream);


        doc.Open();

        foreach (var assessment in assessments)
        {
            iTextSharp.text.Paragraph paragraph = new($"Id: {assessment.Id}, Title: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");

            doc.Add(paragraph);
        }

        doc.Close();
        var array = new MemoryStream(stream.ToArray());
        return array;
    }

    private Stream WriteExcel(IEnumerable<Assessments> assessments)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Assessments");


        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Category";
        worksheet.Cell(1, 3).Value = "Title";
        worksheet.Cell(1, 4).Value = "Director";
        worksheet.Cell(1, 5).Value = "ImageUrl";
        worksheet.Cell(1, 6).Value = "Gender";
        worksheet.Cell(1, 7).Value = "Duration";
        worksheet.Cell(1, 8).Value = "Concluded";
        worksheet.Cell(1, 9).Value = "Comments";
        worksheet.Cell(1, 10).Value = "Created";

        int row = 2;
        int column = 1;
        foreach (var assessment in assessments)
        {
            worksheet.Cell(row, column).Value = assessment.Id;
            worksheet.Cell(row, column).Value = assessment.Category;
            worksheet.Cell(row, column).Value = assessment.Name;
            worksheet.Cell(row, column).Value = assessment.Director;
            worksheet.Cell(row, column).Value = assessment.Image;
            worksheet.Cell(row, column).Value = assessment.Gender;
            worksheet.Cell(row, column).Value = assessment.Duration;
            worksheet.Cell(row, column).Value = assessment.Concluded;
            worksheet.Cell(row, column).Value = assessment.Comments;
            worksheet.Cell(row, column).Value = assessment.Created;
            row++;
            column++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
