using System;
using Drastic.Tray;

namespace Drastic.TrayWindow
{
    public class NSTrayWindow
    {
        private TrayIcon icon;
        private TrayWindowOptions options;
        private NSPopover popover;

        public NSTrayWindow(TrayIcon icon, TrayWindowOptions options, NSViewController? contentViewController = default)
        {
            this.icon = icon;
            this.options = options;

            this.popover = new NSPopover()
            {
                ContentViewController = new NSViewController(),
                Behavior = NSPopoverBehavior.ApplicationDefined,
                Delegate = new NSPopoverDelegate()
            };

            if (contentViewController is not null)
            {
                this.SetContent(contentViewController);
            }
        }

        public void ToggleVisibility()
        {
            if (this.popover.Shown)
            {
                this.popover.Close();
            }
            else
            {
                this.popover.ContentViewController.View.Frame = new CoreGraphics.CGRect(0, 0, options.WindowWidth, options.WindowHeight);
                this.popover.Show(this.icon.StatusBarItem.Button.Bounds, this.icon.StatusBarItem.Button, NSRectEdge.MaxYEdge);
            }
        }

        public void SetContent(NSViewController contentViewController)
        {
            this.popover.ContentViewController = contentViewController;
        }
    }
}