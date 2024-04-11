using Mobile.ViewModels;

namespace Mobile.Views;

public partial class AddCardPage : ContentPage
{
    private AddCardViewModel _vm;
    public AddCardPage(AddCardViewModel vm)
	{
        InitializeComponent();

        BindingContext = vm;
        _vm = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.CleanData();
    }
}