// <copyright file="BaseWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using WinRT;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Base Window.
    /// Used to hold the <see cref="ISystemBackdropControllerWithTargets"/> and enable it when the
    /// Window is created.
    /// </summary>
    public class BaseWindow : Window
    {
        private ISystemBackdropControllerWithTargets? backdropController;
        private SystemBackdropConfiguration? configSource;
        private WindowsSystemDispatcherQueueHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWindow"/> class.
        /// </summary>
        /// <param name="controller"><see cref="ISystemBackdropControllerWithTargets"/>.</param>
        /// <param name="config"><see cref="SystemBackdropConfiguration"/>, Optional.</param>
        public BaseWindow(ISystemBackdropControllerWithTargets? controller = default, SystemBackdropConfiguration? config = default)
        {
            this.helper ??= new WindowsSystemDispatcherQueueHelper();
            this.helper.EnsureWindowsSystemDispatcherQueueController();

            if (controller is null)
            {
                return;
            }

            this.backdropController = controller;
            this.configSource = config ?? new SystemBackdropConfiguration();

            this.Activated += this.AppWindow_Activated;
            this.Closed += this.AppWindow_Closed;

            if (this.Content is FrameworkElement { ActualTheme: var actualTheme })
            {
                this.configSource.Theme = actualTheme switch
                {
                    ElementTheme.Dark => SystemBackdropTheme.Dark,
                    ElementTheme.Light => SystemBackdropTheme.Light,
                    ElementTheme.Default => SystemBackdropTheme.Default,
                    _ => SystemBackdropTheme.Default,
                };
            }

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            this.backdropController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            this.backdropController.SetSystemBackdropConfiguration(this.configSource);
        }

        private void AppWindow_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (this.backdropController != null)
            {
                this.backdropController.Dispose();
                this.backdropController = null;
            }

            this.Activated -= this.AppWindow_Activated;

            this.configSource = null;
        }

        private void AppWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (this.configSource is null)
            {
                return;
            }

            this.configSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;

            if (this.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.ActualThemeChanged += (sender, args) =>
                {
                    this.configSource.Theme = frameworkElement.ActualTheme switch
                    {
                        ElementTheme.Dark => SystemBackdropTheme.Dark,
                        ElementTheme.Light => SystemBackdropTheme.Light,
                        ElementTheme.Default => SystemBackdropTheme.Default,
                        _ => SystemBackdropTheme.Default,
                    };
                };
            }
        }
    }
}
