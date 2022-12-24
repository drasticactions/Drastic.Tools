// <copyright file="PageOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Overlay;

namespace Drastic.PageOverlay
{
    /// <summary>
    /// Page Overlay.
    /// </summary>
    public partial class PageOverlay : WindowOverlay, IVisualTreeElement
    {
        private bool pageOverlayNativeElementsInitialized;
        private HashSet<IHitTestView> views = new HashSet<IHitTestView>();
        private Dictionary<IHitTestView, List<IView>> hitTestElements = new Dictionary<IHitTestView, List<IView>>();
        private IMauiContext? context;

        public PageOverlay(IWindow window)
            : base(window)
        {
        }

        /// <summary>
        /// Gets a read only list of the pages on the overlay.
        /// </summary>
        public IReadOnlyList<IHitTestView> Views => this.views.ToList().AsReadOnly();

        /// <summary>
        /// Gets hit of hit testable elements.
        /// </summary>
        public IReadOnlyList<IView> HitTestElements => this.hitTestElements.SelectMany(n => n.Value).ToList().AsReadOnly();

        /// <summary>
        /// Add View.
        /// </summary>
        /// <param name="view">View.</param>
        /// <returns>Boolean.</returns>
        /// <exception cref="ArgumentNullException">You must base your view on a IHitTestView.</exception>
        public bool AddView(Page view)
        {
            if (view is not IHitTestView hitTestView)
            {
                // You must base your view on a IHitTestView.
                throw new ArgumentException(nameof(view));
            }

            var result = this.views.Add(hitTestView);
            if (!result)
            {
                return false;
            }
#if IOS || WINDOWS || ANDROID || MACCATALYST
            this.AddNativeElements(view);
#endif
            if (view is IHitTestView hittestView)
            {
                this.hitTestElements.Add(hitTestView, hittestView.HitTestViews);
            }

            VisualDiagnostics.OnChildAdded(this, view, 0);
            return true;
        }

        /// <summary>
        /// Remove View.
        /// </summary>
        /// <param name="view">View.</param>
        /// <returns>Bool.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
        public bool RemoveView(Page view)
        {
            if (view is not IHitTestView hitTestView)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var result = this.views.Remove(hitTestView);
            if (!result)
            {
                return false;
            }

#if IOS || WINDOWS || ANDROID || MACCATALYST
            this.RemoveNativeElements(view);
#endif

            VisualDiagnostics.OnChildRemoved(this, view, 0);
            return this.hitTestElements.Remove(hitTestView);
        }

        /// <summary>
        /// Remove Views.
        /// </summary>
        public void RemoveViews()
        {
            this.views.Clear();
            this.hitTestElements.Clear();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IVisualTreeElement> GetVisualChildren()
        {
            var elements = new List<IVisualTreeElement>();
            foreach (var page in this.views)
            {
                if (page is IVisualTreeElement element)
                {
                    elements.AddRange(element.GetVisualChildren());
                }
            }

            return elements;
        }

        /// <inheritdoc/>
        public IVisualTreeElement? GetVisualParent()
        {
            return this.Window as IVisualTreeElement;
        }
    }
}