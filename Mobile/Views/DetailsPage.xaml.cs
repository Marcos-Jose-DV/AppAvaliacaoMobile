using Mobile.ViewModels;

namespace Mobile.Views;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel vm)
	{
		InitializeComponent();

		try
		{
			BindingContext = vm;
		}
		catch (Exception ex)
		{
			ex.Message.ToString();
		}
	}
}