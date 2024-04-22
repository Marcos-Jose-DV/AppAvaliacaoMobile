using ClosedXML.Excel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text.pdf;
using Mobile.Herpels;
using Mobile.Models;
using Mobile.Services;
using Xceed.Words.NET;
using Document = iTextSharp.text.Document;

namespace Mobile.ViewModels;

public partial class DownloadViewModel : ObservableObject
{

    private readonly IFileSaver _fileSaver;
    private readonly AssessmentService _assessmentService;

    public DownloadViewModel(IFileSaver fileSaver, AssessmentService assessmentService)
    {
        _fileSaver = fileSaver;
        _assessmentService = assessmentService;
    }

    [RelayCommand]
    async Task Inport()
    {
        var result = await FileManupulation.FileUplod();

        var assessments = ReadFile.ReadExcel(result.FullPath);

        var ok = await _assessmentService.PostAssessments(assessments);

        Console.WriteLine(ok);
    }
    [RelayCommand]
    async Task SaveFile(string fileName)
    {
        var cancellationToken = new CancellationToken();
        try
        {
            var assessments = await _assessmentService.GetAssessments("");
            Stream stream = null;

            if (fileName.Equals("assessments.txt"))
            {
                stream = WriteTxt(assessments.Item2);
            }
            else if (fileName.Equals("assessments.pdf"))
            {
                stream = WritePDF(assessments.Item2);
            }
            else if (fileName.Equals("assessments.xlsx"))
            {
                stream = WriteExcel(assessments.Item2);
            }
            else
            {
                stream = WriteDoxc(assessments.Item2);
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
            paragraph.Append($"Id: {assessment.Id}, Name: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");

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
            writer.WriteLine($"Id: {assessment.Id}, Name: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");
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
            iTextSharp.text.Paragraph paragraph = new($"Id: {assessment.Id}, Name: {assessment.Name}, Assessment: {assessment.Assessment}, Director: {assessment.Director}, Image: {assessment.Image}, Gender: {assessment.Gender}, Duration: {assessment.Duration}, Concluded: {assessment.Concluded}, Comments: {assessment.Comments}, Category: {assessment.Category}, Created: {assessment.Created}");

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

        worksheet.Cell(1, 1).Value = "Name";
        worksheet.Cell(1, 2).Value = "Nota";
        worksheet.Cell(1, 3).Value = "Categoria";
        worksheet.Cell(1, 4).Value = "Diretor";
        worksheet.Cell(1, 5).Value = "Image";
        worksheet.Cell(1, 6).Value = "Genero";
        worksheet.Cell(1, 7).Value = "Duração";
        worksheet.Cell(1, 8).Value = "Concluido";
        worksheet.Cell(1, 9).Value = "Comentarios";
        worksheet.Cell(1, 10).Value = "Criado";

        int row = 2;
        foreach (var assessment in assessments)
        {
            worksheet.Cell(row, 1).Value = assessment.Name;
            worksheet.Cell(row, 2).Value = assessment.Assessment;
            worksheet.Cell(row, 3).Value = assessment.Category;
            worksheet.Cell(row, 4).Value = assessment.Director;
            worksheet.Cell(row, 5).Value = assessment.Image;
            worksheet.Cell(row, 6).Value = assessment.Gender;
            worksheet.Cell(row, 7).Value = assessment.Duration;
            worksheet.Cell(row, 8).Value = assessment.Concluded;
            worksheet.Cell(row, 9).Value = assessment.Comments;
            worksheet.Cell(row, 10).Value = assessment.Created;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
