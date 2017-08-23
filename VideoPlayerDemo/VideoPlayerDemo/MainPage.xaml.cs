using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VideoPlayerDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            videoPlayer.FullScreenStatusChanged += VideoPlayer_FullScreenStatusChanged;
        }

        private void VideoPlayer_FullScreenStatusChanged(object sender, bool value)
        {
            NavigationPage.SetHasNavigationBar(this, !value);
        }

        //private void Scrollview_Scrolled(object sender, ScrolledEventArgs e)
        //{
        //    videoPlayer.HideSeekbar();
        //}

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    await Task.Delay(4000);
        //    videoPlayer.FullScreen(true);
        //    //await Task.Delay(4000);
        //    //videoPlayer.ExitFullScreen();
        //}

        private async void VideoPlayer_Error(object sender, Xamarians.MediaPlayer.PlayerErrorEventArgs e)
        {
            //await DisplayAlert("Error", e.Message, "OK");
        }

        private async void VideoPlayer_Prepared(object sender, EventArgs e)
        {
            // await DisplayAlert("", "Prepared", "OK");
        }

        private async void VideoPlayer_Completed(object sender, EventArgs e)
        {
            // await DisplayAlert("", "Completed", "OK");

        }
    }
}
