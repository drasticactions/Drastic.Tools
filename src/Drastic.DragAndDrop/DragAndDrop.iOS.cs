// <copyright file="DragAndDrop.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop
{
    public partial class DragAndDrop
    {
        private UIViewController controller;

        private DragAndDropView? dragAndDropView;

        public DragAndDrop(UIWindow window)
            : this(window.RootViewController!)
        {
        }

        public DragAndDrop(UIViewController controller)
        {
            this.controller = controller;

            // We're going to create a new view.
            // This will handle the "drop" events, and nothing else.

            this.dragAndDropView = new DragAndDropView(this, this.controller.View!.Frame);
            this.dragAndDropView.UserInteractionEnabled = true;
            this.controller.View.AddSubview(this.dragAndDropView);
            this.controller.View.BringSubviewToFront(this.dragAndDropView);
        }

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
