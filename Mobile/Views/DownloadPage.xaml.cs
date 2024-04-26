using Microsoft.Maui.ApplicationModel.DataTransfer;
using Mobile.ViewModels;

namespace Mobile.Views;

public partial class DownloadPage : ContentPage
{
	public DownloadPage(DownloadViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


    private void OnDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
    }

    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        var items = e.Data.GetImageAsync();
    }

    private void DropGestureRecognizer_DragLeave(object sender, DragEventArgs e)
    {

    }

    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {

    }

    private void DragGestureRecognizer_DropCompleted(object sender, DropCompletedEventArgs e)
    {

    }
}