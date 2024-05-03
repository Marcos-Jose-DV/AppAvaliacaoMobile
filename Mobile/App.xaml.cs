using Mobile.Constans;
using System.Diagnostics;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string path = Configurations.ApiPath;

            if (!System.IO.File.Exists(path))
            {
                Console.WriteLine("O arquivo executável não foi encontrado.");
            }
            else
            {
                Process process = new Process();
                process.StartInfo.FileName = path;

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro ao tentar executar o executável: {ex.Message}");
                }
            }

            MainPage = new AppShell();
        }
    }
}
