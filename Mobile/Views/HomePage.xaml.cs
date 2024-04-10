using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{

    HomeViewModel ViewModel;
    public HomePage(HomeViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
        ViewModel = vm;        
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.Load();
    }
}