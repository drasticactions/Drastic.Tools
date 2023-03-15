using Drastic.HtmlLabel.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.UI.Xaml;
using System;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace Drastic.HtmlLabel.Maui
{
    internal abstract class Behavior<T> : Behavior where T : DependencyObject
	{
		protected new T AssociatedObject => base.AssociatedObject as T;

		protected override void OnAttached()
		{
			base.OnAttached();
			if (AssociatedObject == null)
            {
                throw new InvalidOperationException("AssociatedObject is not of the right type");
            }
        }
	}
}