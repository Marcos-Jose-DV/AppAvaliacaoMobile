using Mobile.ViewModels;

namespace Mobile.Views;

public partial class DownloadPage : ContentPage
{
	public DownloadPage(DownloadViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}