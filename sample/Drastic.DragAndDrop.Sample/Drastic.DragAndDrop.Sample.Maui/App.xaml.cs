namespace Drastic.DragAndDrop.Sample.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Drastic.DragAndDrop.Maui.DragAndDropWindow(new Drastic.DragAndDrop.Maui.DragElementOverlay(Color.FromRgba(225, 0, 0, .2))) { Page = new AppShell() };
    }
}
