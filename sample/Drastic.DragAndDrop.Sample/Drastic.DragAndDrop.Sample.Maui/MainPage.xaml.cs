using Drastic.DragAndDrop.Maui;

namespace Drastic.DragAndDrop.Sample.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var window = (DragAndDropWindow)this.GetParentWindow();
        window.Drop += Window_Drop;
    }

    private void Window_Drop(object? sender, DragAndDropOverlayTappedEventArgs e)
    {
        try
        {
            // Get the first path.
            if (e.Paths.Any())
            {
                this.DropImage.Source = ImageSource.FromFile(e.Paths[0]);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}