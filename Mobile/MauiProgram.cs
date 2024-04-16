﻿using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
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
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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
            builder.Services.AddScoped<IAssessmentService, AssessmentService>();
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(AddCardPage), typeof(AddCardPage));
            Routing.RegisterRoute(nameof(PlayPage), typeof(PlayPage));



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
                                overlappedPresenter.SetBorderAndTitleBar(true,true);
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
    }
}
