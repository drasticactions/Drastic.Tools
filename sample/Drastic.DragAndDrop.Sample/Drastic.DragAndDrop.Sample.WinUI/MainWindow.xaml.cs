// <copyright file="MainWindow.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Drastic.DragAndDrop.Sample.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private DragAndDrop? dragAndDrop;

        public MainWindow()
        {
            this.InitializeComponent();
            this.dragAndDrop = new DragAndDrop(this);
            this.dragAndDrop.Drop += DragAndDrop_Drop;
            this.dragAndDrop.Dragging += DragAndDrop_Dragging;
        }

        private void DragAndDrop_Dragging(object? sender, DragAndDropIsDraggingEventArgs e)
        {
        }

        private async void DragAndDrop_Drop(object? sender, DragAndDropOverlayTappedEventArgs e)
        {
            try
            {
                // Get the first path.
                if (e.Paths.Any())
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(e.Paths[0]);
                    using IRandomAccessStream fileStream = await file.OpenReadAsync();

                    // Create a bitmap to be the image source.
                    BitmapImage bitmapImage = new();
                    bitmapImage.SetSource(fileStream);

                    this.DropImage.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
