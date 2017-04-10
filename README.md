# MediaPlayer
Cross Platform Media Player to play videos in Xamarin.Forms. Supports android and iOS platforms.

First install package from nuget using following command -
## Install-Package Xamarians.MediaPlayer

You can integrate video player in you Xamarin Form application using following code:

 Shared Code -
 
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:xamarians="clr-namespace:Xamarians.MediaPlayer;assembly=Xamarians.MediaPlayer"
             x:Class="App1.MainPage">

    <xamarians:VideoPlayer Source="http://0.s3.envato.com/h264-video-previews/80fad324-9db4-11e3-bf3d-0050569255a8/490527.mp4" AutoPlay="True">
        
    </xamarians:VideoPlayer>

</ContentPage>

Android - in MainActivity file write below code -
## Xamarians.MediaPlayer.Droid.VideoPlayerRenderer.Init();

iOS - in AppDelegate file write below code -
## Xamarians.MediaPlayer.iOS.VideoPlayerRenderer.Init();
