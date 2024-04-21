using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel _vm;
    int skip = 0;
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
        _vm = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Clean();
    }

    private async void CollectionView_Scrolled2(object sender, ItemsViewScrolledEventArgs e)
    {
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            return;
        }

        if (sender is CollectionView cv)
        {
            if (_vm._pageIndex == ((IEnumerable<object>)cv.ItemsSource).Count())
            {
                skip = 0;
                return;
            }

            var last = e.LastVisibleItemIndex;
            var remainig = cv.RemainingItemsThreshold;
            var total = ((IEnumerable<object>)cv.ItemsSource).Count();

            if (last > (total - remainig))
            {
                skip += 20;
                await _vm.Load(skip);
            }
        }
    }

    private void ShowMenu(object sender, EventArgs e)
    {
        var button = (Button)sender;
        
        double rotateTo = 45;
        if (Menu.IsVisible) rotateTo = 0;
        
        button.RotateTo(rotateTo, 250, Easing.Linear);
        Menu.IsVisible = !Menu.IsVisible;
    }
}