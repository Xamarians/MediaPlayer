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

        private void VideoPlayer_Error(object sender, Xamarians.MediaPlayer.PlayerErrorEventArgs e)
        {
            //await DisplayAlert("Error", e.Message, "OK");
        }

        private void VideoPlayer_Prepared(object sender, EventArgs e)
        {
            // await DisplayAlert("", "Prepared", "OK");
        }

        private void VideoPlayer_Completed(object sender, EventArgs e)
        {
            // await DisplayAlert("", "Completed", "OK");

        }
    }
}
