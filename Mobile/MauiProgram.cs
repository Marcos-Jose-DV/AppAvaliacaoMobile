using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Mobile.Services;
using Mobile.Services.Interfaces;
using Mobile.ViewModels;
using Mobile.Views;

namespace Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<BookPage>();
            builder.Services.AddSingleton<BookViewModel>();
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddSingleton<DetailsViewModel>();
            builder.Services.AddSingleton<EditPage>();
            builder.Services.AddSingleton<EditViewModel>();
            builder.Services.AddSingleton<DownloadPage>();
            builder.Services.AddSingleton<DownloadViewModel>();

            builder.Services.AddScoped<IAssessmentService, AssessmentService>();
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);


            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
