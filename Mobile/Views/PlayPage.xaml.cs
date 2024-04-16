using CommunityToolkit.Maui.Views;
using Mobile.ViewModels;

namespace Mobile.Views;

public partial class PlayPage : ContentPage
{
    public PlayPage(PlayViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
        OnToggleFullscreenClicked();
    }
    private void OnToggleFullscreenClicked()
    {
#if WINDOWS

        var window = App.Current.MainPage.GetParentWindow().Handler.PlatformView as MauiWinUIWindow;

        var appWindow = GetAppWindow(window);

        switch (appWindow.Presenter)
        {
            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                overlappedPresenter.SetBorderAndTitleBar(false, false);
                overlappedPresenter.Maximize();
                overlappedPresenter.IsMaximizable = false;

                break;
        }
    }
#endif

#if WINDOWS
    private Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
        return appWindow;
    }
#endif
    void ContentPage_Unloaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
    }
}