# MediaPlayer
Cross Platform Media Player to play videos in Xamarin.Forms. Supports android and iOS platforms.

First install package from nuget using following command -
## Install-Package Xamarians.MediaPlayer

You can integrate video player in you Xamarin Form application using following code:

 Shared Code -
 
```
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:xamarians="clr-namespace:Xamarians.MediaPlayer;assembly=Xamarians.MediaPlayer"
             x:Class="App1.MainPage">

    <xamarians:VideoPlayer Source="<VIDEO URL>" AutoPlay="True">
        
    </xamarians:VideoPlayer>

</ContentPage>
```

To play video in full screen ,you need to add these lines in MainPage - 
```
       videoPlayer.FullScreenStatusChanged += VideoPlayer_FullScreenStatusChanged;
        
        private void VideoPlayer_FullScreenStatusChanged(object sender, bool value)
        {
            NavigationPage.SetHasNavigationBar(this, !value);
        }
```

Android - in MainActivity file write below code -
```
Xamarians.MediaPlayer.Droid.VideoPlayerRenderer.Init(this);
```

iOS - in AppDelegate file write below code -
```
Xamarians.MediaPlayer.iOS.VideoPlayerRenderer.Init();
```
Also add following lines in info.plist to allow app to open url over http.
```
<key>NSAppTransportSecurity</key>
<dict>
<key>NSAllowsArbitraryLoads</key>
<true/>
</dict>
```


### For any issue with library please report here 
https://github.com/Xamarians/MediaPlayer/issues/new
