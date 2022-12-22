// <copyright file="SampleTrayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.TrayWindow.Sample.Maui
{
    public class SampleTrayWindow : Window
    {
        private TrayWindow.MAUITrayWindow? window;
        private Tray.TrayIcon icon;

        public SampleTrayWindow(Tray.TrayIcon icon)
        {
            this.icon = icon;
        }

        public async Task ToggleVisibilityAsync()
        {
            if (this.window is not null)
            {
                await window.ToggleVisibilityAsync();
            }
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            // The window has been created, we will recon it into a tray window.
            this.window = new MAUITrayWindow((UIKit.UIWindow)this.Handler.PlatformView!, this.icon, new TrayWindowOptions(), ((UIKit.UIWindow)this.Handler.PlatformView!).RootViewController);
        }
    }
}