using DocumentFormat.OpenXml.Spreadsheet;
using Mobile.Constans;
using System.Diagnostics;

namespace Mobile
{
    public partial class App : Application
    {
        Process processo;
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
