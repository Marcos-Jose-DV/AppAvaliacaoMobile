using Microsoft.Maui.Devices;
using Mobile.Views;

namespace Mobile
{
    public partial class App : Application
    {
        private double _width;
        private double _height;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    var window = base.CreateWindow(activationState);
        //    window.Width = DeviceDisplay.Current.MainDisplayInfo.Width;
        //    window.Height = DeviceDisplay.Current.MainDisplayInfo.Height;
        //    return window;
        //}
    }
}
