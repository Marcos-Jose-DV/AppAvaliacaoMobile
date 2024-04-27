using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Mobile.ViewModels;
using Mobile.Views;
using Mobile.Interfaces;
using Mobile.Constans;

namespace Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterServices();


        builder.Services.AddSingleton<IPlatformHttpMessageHandler>(sp =>
        {
#if ANDROID
        			return new Platforms.Android.AndroidHttpMessageHandler();
#elif IOS
                    return new Platforms.iOS.IosHttpMessageHandler();
#elif WINDOWS
                    return new Platforms.Windows.WindowsHttpMessageHandler();
#endif
        });


        builder.Services.AddHttpClient(Configurations.HttpClientName, httpClient =>
        {
            var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7193"
                : "https://localhost:7193";

            httpClient.BaseAddress = new Uri(baseAddress);

        }).ConfigureHttpMessageHandlerBuilder(configBuilder =>
        {
            var platformHttpMessageHandler = configBuilder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
            configBuilder.PrimaryHandler = platformHttpMessageHandler.GetHttpMessageHandler();
        });


#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(windowsLifecycleBuilder =>
            {
                windowsLifecycleBuilder.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = false;
                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                    switch (appWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(true, true);
                            overlappedPresenter.Maximize();
                            overlappedPresenter.IsMaximizable = false;
                            break;
                    }
                });
            });
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<MoviePage>();
        builder.Services.AddSingleton<MovieViewModel>();

        builder.Services.AddTransient<AddCardPage>();
        builder.Services.AddSingleton<AddCardViewModel>();

        builder.Services.AddSingleton<DetailsPage>();
        builder.Services.AddSingleton<DetailsViewModel>();

        builder.Services.AddTransient<PlayPage>();
        builder.Services.AddSingleton<PlayViewModel>();

        builder.Services.AddSingleton<DownloadPage>();
        builder.Services.AddSingleton<DownloadViewModel>();
        builder.Services.AddScoped<Services.AssessmentService>();
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);


        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Routing.RegisterRoute(nameof(AddCardPage), typeof(AddCardPage));
        Routing.RegisterRoute(nameof(DownloadPage), typeof(DownloadPage));
        Routing.RegisterRoute(nameof(PlayPage), typeof(PlayPage));

        return builder;
    }
}

