// <copyright file="Behavior.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.HtmlLabel.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    internal abstract class Behavior : DependencyObject, IBehavior
    {
        protected DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            this.OnAttached();
        }

        public void Detach() => this.OnDetaching();

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetaching()
        {
        }

        DependencyObject IBehavior.AssociatedObject => this.AssociatedObject;
    }
}