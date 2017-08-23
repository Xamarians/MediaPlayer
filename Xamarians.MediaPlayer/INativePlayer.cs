using System;

namespace Xamarians.MediaPlayer
{
    internal interface INativePlayer
    {
        event EventHandler<bool> FullScreenStatusChanged;
        bool IsSeekbarVisible { get; }
        int Duration { get; }
        int CurrentPosition { get; }
        void Play();
        void Pause();
        void Stop();
        void Seek(int seconds);
        void DisplaySeekbar(bool value);
    }
}
