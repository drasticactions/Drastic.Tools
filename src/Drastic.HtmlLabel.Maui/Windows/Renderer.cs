using System.ComponentModel;
using Microsoft.Maui.Controls.Compatibility;
using Drastic.HtmlLabel.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Xaml.Interactivity;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace Drastic.HtmlLabel.Maui
{
	/// <summary>
	/// HtmlLable Implementation
	/// </summary>
	[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
	public class HtmlLabelRenderer : LabelRenderer
	{
		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Initialize() { }

		/// <inheritdoc />
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e == null || Element == null)
			{
				return;
			}

			try
			{
				ProcessText();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
			}
		}

		/// <inheritdoc />
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e != null && RendererHelper.RequireProcess(e.PropertyName))
			{
				try
				{
					ProcessText();
				}
				catch (System.Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
				}
			}
		}

		private void ProcessText()
		{
			if (Control == null || Element == null)
            {
                return;
            }

			// Gets the complete HTML string
			var isRtl = Device.FlowDirection == Microsoft.Maui.FlowDirection.RightToLeft;
			var styledHtml = new RendererHelper(Element, Element.Text, Device.RuntimePlatform, isRtl).ToString();
			if (styledHtml == null)
			{
				return;
			}

			Control.Text = styledHtml;

			// Adds the HtmlTextBehavior because UWP's TextBlock
			// does not natively support HTML content
			var behavior = new HtmlTextBehavior((HtmlLabel)Element);
            BehaviorCollection behaviors = Interaction.GetBehaviors(Control);
			behaviors.Clear();
			behaviors.Add(behavior);
		}
	}
}