using Mobile.ViewModels;

namespace Mobile.Views;

public partial class EditPage : ContentPage
{
	public EditPage(EditViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}