using Drastic.Audio.Model;
using Drastic.Audio.Services;

namespace Drastic.Audio.Sample.Maui;

public partial class MainPage : ContentPage
{
    private NativeMediaService service;
    public MainPage()
    {
        InitializeComponent();
        var media = new SampleMedia() { OnlinePath = new Uri("https://cdn.discordapp.com/attachments/1047080059736952862/1057497416154226768/Upstate_-_TrackTribe.mp3") };
        this.service = new NativeMediaService();
        this.service.CurrentMedia = media;
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await this.service.PlayAsync();
    }

    async void CounterBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.service.PauseAsync();
    }
}

public class SampleMedia : IMediaItem
{
    public string? LocalPath { get; set; }
    public Uri? OnlinePath { get; set; }
}