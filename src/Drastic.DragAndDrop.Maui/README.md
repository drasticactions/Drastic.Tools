[![NuGet Version](https://img.shields.io/nuget/v/Drastic.DragAndDrop.Maui.svg)](https://www.nuget.org/packages/Drastic.DragAndDrop.Maui/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.DragAndDrop.Maui

Drastic.DragAndDrop.Maui is a component for adding drag and drop views and overlay effects to a MAUI Window.

## How To Use

First, you need to create the overlay and add it to a `Window` instance. This can be done to an existing `Window` after it has been created, or in the `Window` itself with code you add.

```c#
 this.overlay = new DragAndDropOverlay(this, this.overlayElement);
this.overlay.Drop += (sender, e) => this.Drop?.Invoke(this, e);

this.AddOverlay(this.overlay);
```

You can also use the `DragAndDropWindow` to automatically add the overlay to a window. Either create your own `DragAndDropWindow` after your app has launched, or replace the following in `App.cs`

```c#
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
```

If you use `DragAndDropWindow`, you can get the reference to it from the page you're on, and respond to the `Drop` event.

```c#
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
```

If you're using the `DragAndDropOverlay` directly, you can attach to its `Drop` event.

# Tips

MAUI already has [Drag and Drop](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/gestures/drag-and-drop?view=net-maui-7.0) gestures for individual elements. If you wish, you can also use `Drastic.DragAndDrop` directly and subscribe to a MAUI Views `PlatforView` and add a DragAndDrop control to that.