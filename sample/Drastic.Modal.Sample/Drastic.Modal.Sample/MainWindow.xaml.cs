// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Drastic.Modal.Sample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    [CommunityToolkit.Mvvm.ComponentModel.INotifyPropertyChanged]
    public sealed partial class MainWindow : Window, IModalWindowEventHandler
    {
        [ObservableProperty]
        private bool isDisabled;

        public MainWindow()
        {
            this.InitializeComponent();
            this.DisabledRectangle.DataContext = this;
            //this.ExtendsContentIntoTitleBar = true;
            this.ExtendsContentIntoAppTitleBar(true);
            this.SetTitleBar(this.AppTitleBar);
        }

        void IModalWindowEventHandler.Disabled()
        {
            this.IsDisabled = true;
        }

        void IModalWindowEventHandler.Enabled()
        {
            this.IsDisabled = false;
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            var position = this.GetPosition();
            var size = new Windows.Graphics.SizeInt32(500, 600);
            var testModal = new TestModalWindow(this, new ModalWindowOptions() { MinSize = size, Position = this.PositionModalInCenter(size) });
            testModal.Activate();
        }
    }
}
