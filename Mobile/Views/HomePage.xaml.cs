using DocumentFormat.OpenXml.Drawing;
using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel _vm;
    int skip = 15;
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
        if (((IEnumerable<object>)CollectionViewControl.ItemsSource).Count() == 34)
        {
            skip = 0;
            return;
        }

        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            return;
        }

        if (sender is CollectionView cv)
        {
            var last = e.LastVisibleItemIndex;
            var remainig = cv.RemainingItemsThreshold;
            var total = ((IEnumerable<object>)cv.ItemsSource).Count();

            if (last > (total - remainig))
            {
                await _vm.Load(skip);
                skip += skip;
            }
        }
    }
}