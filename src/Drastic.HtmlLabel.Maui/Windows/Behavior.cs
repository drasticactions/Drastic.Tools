using Microsoft.Xaml.Interactivity;
using Microsoft.Maui.Controls.Compatibility;
using Drastic.HtmlLabel.Maui;
using Microsoft.UI.Xaml;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace Drastic.HtmlLabel.Maui
{
    internal abstract class Behavior : DependencyObject, IBehavior
	{
		public void Attach(DependencyObject associatedObject)
		{
			AssociatedObject = associatedObject;
			OnAttached();
		}

		public void Detach() => OnDetaching();

		protected virtual void OnAttached() { }

		protected virtual void OnDetaching() { }

		protected DependencyObject AssociatedObject { get; set; }

		DependencyObject IBehavior.AssociatedObject => AssociatedObject;
	}
}