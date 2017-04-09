# MediaPlayer
Cross Platform Media Player to play videos in Xamarin.Forms. Supports android and iOS platforms.

First install package from nuget using following command

Install-Package Xamarians.MediaPlayer

Using Xamarin.Forms in you project, you can integrate video player using following code:

<xamarians:VideoPlayer Source="http://0.s3.envato.com/h264-video-previews/80fad324-9db4-11e3-bf3d-0050569255a8/490527.mp4" AutoPlay="True">
    
</xamarians:VideoPlayer>
Android - in MainActivity file write below code -

Xamarians.MediaPlayer.Droid.VideoPlayerRenderer.Init();

iOS - in AppDelegate file write below code -

Xamarians.MediaPlayer.iOS.VideoPlayerRenderer.Init();
