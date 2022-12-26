// <copyright file="DragAndDrop.Mac.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using AppKit;

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// Drag And Drop View.
    /// </summary>
    public partial class DragAndDrop
    {
        private NSView view;
        private DragAndDropView? dragAndDropView;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDrop"/> class.
        /// </summary>
        /// <param name="controller">UIViewController, sets the drag and drop view to the inner View.</param>
        public DragAndDrop(NSViewController controller)
            : this(controller.View!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDrop"/> class.
        /// </summary>
        /// <param name="view">Sets the drag and drop view to the UIView.</param>
        public DragAndDrop(NSView view)
        {
            this.view = view;
            this.dragAndDropView = new DragAndDropView(this, this.view.Frame);
            this.dragAndDropView.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;

            this.view.AddSubview(this.dragAndDropView);
            this.view.BringSubviewToFront(this.dragAndDropView);
        }

        private class DragAndDropView : NSView
        {
            private readonly DragAndDrop overlay;

            public DragAndDropView(DragAndDrop overlay, CGRect frame)
                : base(frame)
            {
                this.overlay = overlay;
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
                this.RegisterForDraggedTypes(new string[] { NSPasteboard.NSFilenamesType });
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
            }

            /// <summary>
            /// Dragging Entered.
            /// </summary>
            /// <param name="sender"><see cref="INSDraggingInfo"/>.</param>
            [Export("draggingEntered:")]
            public NSDragOperation DraggingEntered(INSDraggingInfo sender)
            {
                var filenames = this.DraggedFilenames(sender.DraggingPasteboard).ToList() ?? new List<string>();
                this.overlay.Dragging?.Invoke(this, new DragAndDropIsDraggingEventArgs(true, filenames));
                return NSDragOperation.Copy;
            }

            /// <summary>
            /// Dragging Updated.
            /// </summary>
            /// <param name="sender"><see cref="INSDraggingInfo"/>.</param>
            [Export("draggingUpdated:")]
            public NSDragOperation DraggingUpdated(INSDraggingInfo sender)
            {
                return NSDragOperation.None;
            }

            /// <summary>
            /// Dragging Ended.
            /// </summary>
            /// <param name="sender"><see cref="INSDraggingInfo"/>.</param>
            [Export("draggingEnded:")]
            public void DraggingEnded(INSDraggingInfo sender)
            {
                var filenames = this.DraggedFilenames(sender.DraggingPasteboard).ToList() ?? new List<string>();
                if (filenames.Any())
                {
                    this.overlay.Drop?.Invoke(this, new DragAndDropOverlayTappedEventArgs(filenames));
                }

                this.overlay.Dragging?.Invoke(this, new DragAndDropIsDraggingEventArgs(false, filenames));
            }

            /// <summary>
            /// Gets the dragged filenames from the pastboard.
            /// </summary>
            /// <param name="pasteboard">NSPasteBoard.</param>
            /// <returns>List of Filenames.</returns>
            IEnumerable<string> DraggedFilenames(NSPasteboard pasteboard)
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
                if (Array.IndexOf(pasteboard.Types, NSPasteboard.NSFilenamesType) < 0)
                {
                    yield break;
                }
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
                foreach (var i in pasteboard.PasteboardItems)
                {
                    yield return new NSUrl(i.GetStringForType("public.file-url")).Path ?? string.Empty;
                }
            }
        }
    }
}