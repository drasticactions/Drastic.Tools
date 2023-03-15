namespace Drastic.HtmlLabel.Maui.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		this.TestingLabel.Text = @"<u>Testing!</u>";
	}
}