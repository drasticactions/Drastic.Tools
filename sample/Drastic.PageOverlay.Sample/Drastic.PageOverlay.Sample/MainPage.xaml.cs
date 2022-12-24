namespace Drastic.PageOverlay.Sample;

public partial class MainPage : ContentPage
{
    private PageOverlayWindow? window;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.window!.PageOverlay.AddView(new PageOverlaySample(this.window));
    }

    /// <inheritdoc/>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.window = this.GetParentWindow() as PageOverlayWindow;
        if (this.window is null)
        {
            return;
        }
    }
}