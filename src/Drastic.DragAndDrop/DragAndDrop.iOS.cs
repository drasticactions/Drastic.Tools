// <copyright file="DragAndDrop.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// Drag And Drop View.
    /// </summary>
    public partial class DragAndDrop
    {
        private UIView view;

        private DragAndDropView? dragAndDropView;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDrop"/> class.
        /// </summary>
        /// <param name="window">UIWindow. Sets the context of the DragAndDrop view to the inner RootViewController view.</param>
        public DragAndDrop(UIWindow window)
            : this(window.RootViewController!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDrop"/> class.
        /// </summary>
        /// <param name="controller">UIViewController, sets the drag and drop view to the inner View.</param>
        public DragAndDrop(UIViewController controller)
            : this(controller.View!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDrop"/> class.
        /// </summary>
        /// <param name="view">Sets the drag and drop view to the UIView.</param>
        public DragAndDrop(UIView view)
        {
            this.view = view;

            // We're going to create a new view.
            // This will handle the "drop" events, and nothing else.
            this.dragAndDropView = new DragAndDropView(this, this.view.Frame);
            this.dragAndDropView.UserInteractionEnabled = true;
            this.view.AddSubview(this.dragAndDropView);
            this.view.BringSubviewToFront(this.dragAndDropView);
        }

        /// <summary>
        /// Dispose Elements.
        /// </summary>
        internal void DisposeNativeElements()
        {
            if (this.dragAndDropView != null)
            {
                this.dragAndDropView.RemoveFromSuperview();
                this.dragAndDropView.Dispose();
            }
        }

        private class DragAndDropView : UIView, IUIDropInteractionDelegate
        {
            private readonly DragAndDrop overlay;

            public DragAndDropView(DragAndDrop overlay, CGRect frame)
                : base(frame)
            {
                this.overlay = overlay;
                this.AddInteraction(new UIDropInteraction(this) { AllowsSimultaneousDropSessions = true });
            }

            [Export("dropInteraction:canHandleSession:")]
            public bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
            {
                return true;
            }

            [Export("dropInteraction:sessionDidEnter:")]
            public void SessionDidEnter(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = true;
            }

            [Export("dropInteraction:sessionDidExit:")]
            public void SessionDidExit(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = false;
            }

            [Export("dropInteraction:sessionDidUpdate:")]
            public UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
            {
                if (session.LocalDragSession == null)
                {
                    return new UIDropProposal(UIDropOperation.Copy);
                }

                return new UIDropProposal(UIDropOperation.Cancel);
            }

            [Export("dropInteraction:performDrop:")]
            public async void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                session.ProgressIndicatorStyle = UIDropSessionProgressIndicatorStyle.None;
                var filePaths = new List<string>();
                foreach (UIDragItem item in session.Items)
                {
                    var result = await this.LoadItemAsync(item.ItemProvider, item.ItemProvider.RegisteredTypeIdentifiers.ToList());
                    if (result?.FileUrl.Path is string path)
                    {
                        filePaths.Add(path);
                    }
                }

                this.overlay.Drop?.Invoke(this, new DragAndDropOverlayTappedEventArgs(filePaths));
            }

            [Export("dropInteraction:concludeDrop:")]
            public void ConcludeDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = false;
            }

            public override bool PointInside(CGPoint point, UIEvent? uievent)
            {
                // Event 9 is the combination drag and drop event.
                if (uievent != null && (long)uievent.Type == 9)
                {
                    return true;
                }

                return false;
            }

            private async Task<LoadInPlaceResult?> LoadItemAsync(NSItemProvider itemProvider, List<string> typeIdentifiers)
            {
                if (typeIdentifiers is null || !typeIdentifiers.Any())
                {
                    return null;
                }

                var typeIdent = typeIdentifiers.First();

                if (itemProvider.HasItemConformingTo(typeIdent))
                {
                    return await itemProvider.LoadInPlaceFileRepresentationAsync(typeIdent);
                }

                typeIdentifiers.Remove(typeIdent);

                return await this.LoadItemAsync(itemProvider, typeIdentifiers);
            }
        }
    }
}
