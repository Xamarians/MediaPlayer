namespace Xamarians.MediaPlayer
{
    internal interface INativePlayer
    {
         bool IsSeekbarVisible { get; }
        //bool IsFullScreen { get; }
        int Duration { get; }
        int CurrentPosition { get; }
        void Play();
        void Pause();
        void Stop();
        void Seek(int seconds);
       // void FullScreen(bool resizeLayout = false);
       // void ExitFullScreen();
        void DisplaySeekbar(bool value);
    }
}
