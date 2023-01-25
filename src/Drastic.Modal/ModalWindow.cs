// <copyright file="ModalWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace Drastic.Modal
{
    public abstract partial class ModalWindow : Window
    {
        private ModalWindowOptions options;
        private AppWindow window;
        private List<Window> currentWindows;
        private bool isCreated;

        public ModalWindow(Window window, ModalWindowOptions? options = default)
            : this(new List<Window>() { window }, options)
        {
        }

        public ModalWindow(List<Window>? applicationWindows = default, ModalWindowOptions? options = default)
        {
            this.currentWindows = applicationWindows ??= new List<Window>();
            this.options = options ??= new ModalWindowOptions();


            this.window = this.GetAppWindow();

            if (this.window.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = options.IsResizable;
                presenter.IsMaximizable = options.IsMaximizable;
                presenter.IsMinimizable = options.IsMinimizable;
            }

            this.window.IsShownInSwitchers = options.IsShownInSwitchers;

            if (this.options.HideCloseButton)
            {
                InternalModalWindowExtensions.SetWindowLong(this, WindowLongIndexFlags.GWL_STYLE, SetWindowLongFlags.WS_EX_OVERLAPPEDWINDOW | SetWindowLongFlags.WS_CAPTION);
            }

            this.Activated += this.ModalWindow_Activated;
            this.window.Closing += this.Window_Closing;

            if (this.options.MinSize is not null)
            {
                this.window.Resize(this.options.MinSize ?? throw new NullReferenceException(nameof(this.options.MinSize)));
            }

            if (this.options.Position is not null)
            {
                this.window.Move(this.options.Position ?? throw new NullReferenceException(nameof(this.options.Position)));
            }
        }

        private void Window_Closing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            this.SetupApplicationWindows(true);
        }

        private void SetupApplicationWindows(bool enable)
        {
            if (!this.options.DisableOtherWindows)
            {
                return;
            }

            this.currentWindows.SetEnableWindows(enable);
        }

        private void ModalWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (!this.isCreated)
            {
                this.isCreated = true;
                this.Activated -= this.ModalWindow_Activated;
                this.SetupApplicationWindows(false);
            }
        }
    }
}
