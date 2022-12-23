// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing;
using System.Reflection;
using Drastic.Tray;
using Microsoft.UI.Xaml;

namespace Drastic.TrayWindow.Sample.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private TrayIcon icon;
        private WinUITrayWindow trayWindow;

        private Window? window;

        public App()
        {
            this.InitializeComponent();
            var bitmap = Image.FromStream(GetResourceFileContent("TrayIcon.ico")!);
            var trayImage = new TrayImage(bitmap);
            var menuItems = new List<TrayMenuItem>
            {
                new TrayMenuItem("Hello!", trayImage, async () => { }),
                new TrayMenuItem("From!", trayImage, async () => { }),
                new TrayMenuItem("Windows!", trayImage, async () => { }),
            };
            this.icon = new TrayIcon("Tray Icon", trayImage, menuItems);
            this.trayWindow = new SampleTrayWindow(this.icon, new TrayWindowOptions(500, 700));
            this.icon.RightClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Right Click!");
            };
            this.icon.LeftClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                this.trayWindow.ToggleVisibility();
            };
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        public static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Drastic.TrayWindow.Sample.WinUI." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
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
    }
}
