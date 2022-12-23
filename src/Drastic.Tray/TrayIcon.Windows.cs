// <copyright file="TrayIcon.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Icon.
    /// </summary>
    public partial class TrayIcon
    {
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private NotifyIcon notifyIcon;
        private Icon icon;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> class.
        /// </summary>
        /// <param name="name">Icon Title.</param>
        /// <param name="image">Icon Image Stream. Optional.</param>
        /// <param name="menuItems">Items to populate context menu. Optional.</param>
        public TrayIcon(string name, TrayImage image, List<TrayMenuItem>? menuItems = null)
        {
            var test = new Bitmap(image?.Image!);
            this.icon = Icon.FromHandle(test.GetHicon());
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = this.icon;
            this.notifyIcon.Text = name;
            this.menuItems = menuItems ?? new List<TrayMenuItem>();
            this.contextMenuStrip = new ContextMenuStrip();
            this.contextMenuStrip.ItemClicked += this.ContextMenuStrip_ItemClicked;
            var items = this.menuItems.Select(n => this.GenerateItem(n)).Reverse().ToArray();
            this.contextMenuStrip.Items.AddRange(items);
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.MouseClick += this.NotifyIcon_MouseClick;
            this.notifyIcon.Visible = true;
        }

        private void ContextMenuStrip_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem is DrasticToolStripMenuItem stripItem)
            {
                stripItem.Item.Action?.Invoke();
            }
        }

        private void NativeElementDispose()
        {
            this.notifyIcon?.Dispose();
            this.icon?.Dispose();
        }

        private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.LeftClicked?.Invoke(this, TrayClickedEventArgs.Empty);
            }

            if (e.Button == MouseButtons.Right)
            {
                this.RightClicked?.Invoke(this, TrayClickedEventArgs.Empty);
            }
        }

        private DrasticToolStripMenuItem GenerateItem(TrayMenuItem item)
        {
            var menu = new DrasticToolStripMenuItem(item);
            menu.Text = item.Text;
            if (item.Icon is not null)
            {
                menu.Image = item.Icon.Image;
            }

            return menu;
        }

        private class DrasticToolStripMenuItem : ToolStripMenuItem
        {
            public DrasticToolStripMenuItem(TrayMenuItem item)
            {
                this.Text = item.Text;
                if (item.Icon is not null)
                {
                    this.Image = item.Icon.Image;
                }

                this.Item = item;
            }

            public TrayMenuItem Item { get; }
        }
    }
}
