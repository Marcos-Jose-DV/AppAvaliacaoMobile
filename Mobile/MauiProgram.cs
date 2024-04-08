using Microsoft.Extensions.Logging;
using Mobile.Models;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<DetailsPage>();
            builder.Services.AddSingleton<DetailsViewModel>();
            builder.Services.AddSingleton<EditPage>();
            builder.Services.AddSingleton<EditViewModel>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IBookService, BookService>();


            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
