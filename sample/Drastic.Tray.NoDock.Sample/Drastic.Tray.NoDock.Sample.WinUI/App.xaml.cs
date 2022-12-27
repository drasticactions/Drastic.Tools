// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Drastic.TrayWindow;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Drastic.Tray.NoDock.Sample.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private TrayIcon icon;
        private WinUITrayWindow window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            var trayImage = new TrayImage(System.Drawing.Image.FromStream(GetResourceFileContent("TrayIcon.ico")!));
            var menuItems = new List<TrayMenuItem>
            {
                new TrayMenuItem("Quit!", trayImage, async () => { 
                    Application.Current.Exit(); 
                }),
                new TrayMenuItem("Hello!", trayImage, async () => { }),
                new TrayMenuItem("From!", trayImage, async () => { }),
                new TrayMenuItem("Windows!", trayImage, async () => { }),
            };

            this.icon = new TrayIcon("Tray Icon", trayImage, menuItems);
            this.window = new MainWindow(icon, new TrayWindowOptions());
            this.icon.LeftClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                this.window.ToggleVisibility();
            };
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
        }

        private Window? m_window;

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        public static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Drastic.Tray.NoDock.Sample.WinUI." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
