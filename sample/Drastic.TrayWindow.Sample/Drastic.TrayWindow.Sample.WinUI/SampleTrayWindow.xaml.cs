// <copyright file="SampleTrayWindow.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Tray;
using Microsoft.UI.Xaml;

namespace Drastic.TrayWindow.Sample.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SampleTrayWindow : WinUITrayWindow
    {
        public SampleTrayWindow(TrayIcon icon, TrayWindowOptions options)
            : base(icon, options)
        {
            this.InitializeComponent();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
