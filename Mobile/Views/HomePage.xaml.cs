using Mobile.ViewModels;

namespace Mobile.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}