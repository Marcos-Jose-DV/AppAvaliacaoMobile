using Models.Models;

namespace Helpers.FileHelper;

public static class SaveFile
{
    public static Stream Save(string fileName, IEnumerable<Assessments> assessments)
    {
        Stream stream = null;

        if (fileName.Equals("assessments.txt"))
        {
            stream = WriteFile.WriteTxt(assessments);
        }
        else if (fileName.Equals("assessments.pdf"))
        {
            stream = WriteFile.WritePDF(assessments);
        }
        else if (fileName.Equals("assessments.xlsx"))
        {
            stream = WriteFile.WriteExcel(assessments);
        }
        else
        {
            stream = WriteFile.WriteDoxc(assessments);
        }

        return stream;
    }
}
