using CommunityToolkit.Maui.Views;
using Mobile.ViewModels;

namespace Mobile.Views;

public partial class PlayPage : ContentPage
{
	public PlayPage(PlayViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
    void ContentPage_Unloaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
    }
}