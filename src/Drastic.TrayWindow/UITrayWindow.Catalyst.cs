// <copyright file="UITrayWindow.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Tray;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// The handler for the TrayWindow for Mac Catalyst apps.
    /// </summary>
    internal class UITrayWindow : UIWindow
    {
        private TrayIcon icon;
        private TrayViewController trayViewController;

        /// <summary>
        /// Initializes a new instance of the <see cref="UITrayWindow"/> class.
        /// </summary>
        /// <param name="scene"><see cref="UIScene"/>.</param>
        /// <param name="icon"><see cref="TrayIcon"/>.</param>
        /// <param name="options"><see cref="TrayWindowOptions"/>.</param>
        /// <param name="contentViewController">The Content Controller to inject into the given view.</param>
        public UITrayWindow(UIWindowScene scene, TrayIcon icon, TrayWindowOptions options, UIViewController? contentViewController = default)
            : base(scene)
        {
            this.icon = icon;
            this.RootViewController = this.trayViewController = new TrayViewController(this, icon, options, contentViewController);
        }

        /// <summary>
        /// Toggle the visibility of the window.
        /// </summary>
        public void ToggleVisibility()
            => this.trayViewController.ToggleVisibility();

        /// <summary>
        /// Set the content of the underlying Window.
        /// </summary>
        /// <param name="contentViewController">The Content Controller to inject into the given view.</param>
        public void SetContent(UIViewController contentViewController)
            => this.trayViewController.SetContent(contentViewController);

        /// <summary>
        /// Tray View Controller.
        /// </summary>
        internal class TrayViewController : UIViewController
        {
            private TrayIcon icon;
            private UIImage? image;
            private UIViewController? contentViewController;
            private UIWindow window;
            private UINSWindow? uinsWindow;
            private TrayWindowOptions options;

            /// <summary>
            /// Initializes a new instance of the <see cref="TrayViewController"/> class.
            /// </summary>
            /// <param name="window">The UIWindow.</param>
            /// <param name="icon"><see cref="TrayIcon"/>.</param>
            /// <param name="options"><see cref="TrayWindowOptions"/>.</param>
            /// <param name="contentViewController">The Content Controller to inject into the given view.</param>
            public TrayViewController(UIWindow window, TrayIcon icon, TrayWindowOptions options, UIViewController? contentViewController = default)
            {
                this.icon = icon;
                this.window = window;
                this.options = options;
                this.contentViewController = contentViewController;
                this.image = UIImage.GetSystemImage("cursorarrow.click.2");
                this.SetupWindow();
            }

            /// <summary>
            /// Set the content of the view.
            /// </summary>
            /// <param name="contentViewController">The Content Controller to inject into the given view.</param>
            public void SetContent(UIViewController contentViewController)
            {
                this.contentViewController = contentViewController;
            }

            /// <summary>
            /// Toggle Visibility of the window.
            /// </summary>
            public void ToggleVisibility()
            {
                if (this.uinsWindow is null)
                {
                    return;
                }

                if (this.contentViewController?.View is null)
                {
                    return;
                }

                if (this.View is null)
                {
                    return;
                }

                var buttonBounds = this.icon.GetFrame();
                this.uinsWindow.SetFrameForUIWindow(buttonBounds);
                var viewController = this.contentViewController;

                if (viewController.PresentingViewController is not null)
                {
                    viewController.DismissViewController(true, null);
                }
                else
                {
                    viewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                    viewController.PopoverPresentationController!.SourceView = this.View;
                    viewController.PopoverPresentationController.SourceRect = new CGRect(0, 0, 1, 1);
                    viewController.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    this.PresentViewController(viewController, true, null);
                }
            }

            private async void PrepareForAppearance()
            {
                await this.window.ToggleTitleBarButtons(true);
            }

            private void ForceContentViewLayout()
            {
                if (this.contentViewController?.View is not null)
                {
                    this.View?.AddSubview(this.contentViewController.View);
                    this.contentViewController.View.Frame = new CoreGraphics.CGRect(0, 0, this.options.WindowWidth, this.options.WindowHeight);
                    this.View?.LayoutIfNeeded();
                    this.contentViewController.View.RemoveFromSuperview();
                }
            }

            private async void SetupWindow()
            {
                this.window.RootViewController = this;
#pragma warning disable CA1416 // Validate platform compatibility
                if (this.window.WindowScene?.Titlebar is null)
#pragma warning restore CA1416 // Validate platform compatibility
                {
                    return;
                }

                if (this.window.WindowScene?.SizeRestrictions is null)
                {
                    return;
                }

#pragma warning disable CA1416 // Validate platform compatibility
                this.window.WindowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
#pragma warning restore CA1416 // Validate platform compatibility
                this.window.WindowScene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(1, 1);
                this.window.WindowScene.SizeRestrictions.MaximumSize = new CoreGraphics.CGSize(1, 1);

                this.uinsWindow = await this.window.ToUINSWindowAsync();
                this.uinsWindow!.Level = NSWindowLevel.Floating;
            }
        }
    }
}