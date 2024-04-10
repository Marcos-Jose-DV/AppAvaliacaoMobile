using Mobile.ViewModels;

namespace Mobile.Views;

public partial class BookPage : ContentPage
{

	public BookPage(HomeViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
        vm.Load();
    }
}