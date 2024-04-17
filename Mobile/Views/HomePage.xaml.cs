using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel _vm;
    int skip = 20;
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
            if (_vm._pageIndex == ((IEnumerable<object>)cv.ItemsSource).Count()) { return; }

            var last = e.LastVisibleItemIndex;
            var remainig = cv.RemainingItemsThreshold;
            var total = ((IEnumerable<object>)cv.ItemsSource).Count();

            if (last > (total - remainig))
            {
                await _vm.Load(skip);
                skip += 20;
            }
        }
    }
}