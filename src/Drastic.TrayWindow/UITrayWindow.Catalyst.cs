// <copyright file="UITrayWindow.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Tray;
using static Drastic.TrayWindow.UITrayWindow;

namespace Drastic.TrayWindow
{
    public class MAUITrayWindow
    {
        private TrayIcon icon;
        private TrayViewController trayViewController;
        private UIWindow window;

        public MAUITrayWindow(UIWindow window, TrayIcon icon, TrayWindowOptions options, UIViewController? contentViewController = default)
        {
            this.window = window;
            this.icon = icon;
            this.window.RootViewController = this.trayViewController = new TrayViewController(this.window, icon, options, contentViewController);
        }

        public Task ToggleVisibilityAsync()
            => this.trayViewController.ToggleVisibilityAsync();

        public void SetContent(UIViewController contentViewController)
            => this.trayViewController.SetContent(contentViewController);
    }

    public class UITrayWindow : UIWindow
    {
        private TrayIcon icon;
        private TrayViewController trayViewController;

        public UITrayWindow(UIWindowScene scene, TrayIcon icon, TrayWindowOptions options, UIViewController? contentViewController = default)
            : base (scene)
        {
            this.icon = icon;
            this.RootViewController = this.trayViewController = new TrayViewController(this, icon, options, contentViewController);
        }

        public Task ToggleVisibilityAsync()
            => this.trayViewController.ToggleVisibilityAsync();

        public void SetContent(UIViewController contentViewController)
            => this.trayViewController.SetContent(contentViewController);

        internal class TrayViewController : UIViewController
        {
            private TrayIcon icon;
            private UIImage? image;
            private UIViewController? contentViewController;
            private UIWindow window;
            private UINSWindow? uinsWindow;
            private TrayWindowOptions options;

            public TrayViewController(UIWindow window, TrayIcon icon, TrayWindowOptions options, UIViewController? contentViewController = default)
            {
                this.icon = icon;
                this.window = window;
                this.options = options;
                this.contentViewController = contentViewController;
                this.image = UIImage.GetSystemImage("cursorarrow.click.2");
                this.SetupWindow();
            }

            public void SetContent(UIViewController contentViewController)
            {
                this.contentViewController = contentViewController;
            }

            /// <summary>
            /// Toggle Visibility of the window.
            /// </summary>
            public async Task ToggleVisibilityAsync()
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
                if (this.window.WindowScene?.Titlebar is null)
                {
                    return;
                }

                if (this.window.WindowScene?.SizeRestrictions is null)
                {
                    return;
                }

                this.window.WindowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
                this.window.WindowScene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(1, 1);
                this.window.WindowScene.SizeRestrictions.MaximumSize = new CoreGraphics.CGSize(1, 1);

                this.uinsWindow = await this.window.ToUINSWindowAsync();
                this.uinsWindow!.Level = NSWindowLevel.Floating;
            }
        }
    }
}