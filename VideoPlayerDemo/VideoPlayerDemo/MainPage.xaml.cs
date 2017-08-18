using System;
using Xamarin.Forms;

namespace VideoPlayerDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            scrollview.Scrolled += Scrollview_Scrolled;
        }

        private void Scrollview_Scrolled(object sender, ScrolledEventArgs e)
        {
            videoPlayer.HidePlayerController();
        }

        private async void VideoPlayer_Error(object sender, Xamarians.MediaPlayer.PlayerErrorEventArgs e)
        {
            await DisplayAlert("Error", e.Message, "OK");
        }

        private async void VideoPlayer_Prepared(object sender, EventArgs e)
        {
            await DisplayAlert("", "Prepared", "OK");
        }

        private async void VideoPlayer_Completed(object sender, EventArgs e)
        {
            await DisplayAlert("", "Completed", "OK");

        }
    }
}
