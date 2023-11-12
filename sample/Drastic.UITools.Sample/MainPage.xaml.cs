namespace Drastic.UITools.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();

        var testStringList = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            testStringList.Add($"Test {i}");
        }

        this.TestCollectionView.ItemsSource = testStringList;
    }
}