using Mobile.ViewModels;

namespace Mobile.Views;

public partial class MoviePage : ContentPage
{
	MovieViewModel _vm;
	public MoviePage(MovieViewModel vm)
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
}