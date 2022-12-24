[![NuGet Version](https://img.shields.io/nuget/v/Drastic.DragAndDrop.svg)](https://www.nuget.org/packages/Drastic.DragAndDrop/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.DragAndDrop

Drastic.DragAndDrop is a simple API for adding Drag And Drop enabled views to existing controls. It is not intended to be the most "comprehensive" control, but to be an easy way to add basic functionality to an app. It can also serve as a springboard for your own controls.

![example](https://user-images.githubusercontent.com/898335/209426614-d6a21f90-245e-4f90-8f32-bcb70cb529db.gif)

# How To Use

iOS/Catalyst

```c#
    internal class SampleViewController : UIViewController
    {
        private UILabel label = new UILabel() { Text = "Drag and Drop an image onto the window..." };
        private UIImageView image = new UIImageView() { BackgroundColor = UIColor.Gray };
        private DragAndDrop dragAnDrop;

        public SampleViewController()
        {
            this.SetupUI();
            this.SetupLayout();

            this.dragAnDrop = new DragAndDrop(this);
            this.dragAnDrop.Drop += this.DragAnDrop_Drop;
        }
    ...
```

WinUI

```c#
public sealed partial class MainWindow : Window
    {
        private DragAndDrop? dragAndDrop;

        public MainWindow()
        {
            this.InitializeComponent();
            this.dragAndDrop = new DragAndDrop(this);
            this.dragAndDrop.Drop += DragAndDrop_Drop;
        }

        private async void DragAndDrop_Drop(object? sender, DragAndDropOverlayTappedEventArgs e)
```

# Tips

`DragAndDrop` can be applied to individual `UIView`s for UIKit, or `UIElement`s for WinUI. When you apply the Drag And Drop view to a WinUI Window, it uses the internal contents of the frame as the drop zone. If your content does not extend to the edge of the `Window` (As it does in the default sample) then the drag and drop target will not extend to the edge of the window. If you wish to do that, nest your content within something like a `Grid` that extends to the edge of the frame.