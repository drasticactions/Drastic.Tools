// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;
using Drastic.Tray;
using Microsoft.UI.Xaml;

namespace Drastic.Tray.Sample.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private TrayIcon icon;

        private Window? window;

        public App()
        {
            var trayImage = new TrayImage(GetResourceFileContent("TrayIcon.ico")!);
            var menuItems = new List<TrayMenuItem>
            {
                new TrayMenuItem("Hello!", trayImage, async () => { }),
                new TrayMenuItem("From!", trayImage, async () => { }),
                new TrayMenuItem("Windows!", trayImage, async () => { }),
            };
            this.icon = new TrayIcon("Tray Icon", trayImage, menuItems);
            this.icon.RightClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Right Click!");
            };
            this.icon.LeftClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Left Click!");
            };
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            this.window = new MainWindow();
            this.window.Activate();
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        public static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Drastic.Tray.Sample.WinUI." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
