using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel _viewModel;
    DetailsViewModel _details;
    public HomePage(HomeViewModel vm, DetailsViewModel details)
    {
        InitializeComponent();

        BindingContext = vm;
        _viewModel = vm;
        _details = details;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Load();

        _details.DisposeAsyncMemory();
    }
}