using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Preserve(AllMembers = true)]
namespace Xamarians.MediaPlayer
{
    public class VideoPlayer : Grid
    {
        INativePlayer _nativePlayer;
        Image imgFullScreen;
        bool isPortrait = false;
        #region Properties

        public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(string), typeof(VideoPlayer), null);
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create("AutoPlay", typeof(bool), typeof(VideoPlayer), false);
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public int Duration
        {
            get { return _nativePlayer == null ? 0 : _nativePlayer.Duration; }
        }

        public int CurrentPosition
        {
            get { return _nativePlayer == null ? 0 : _nativePlayer.CurrentPosition; }
        }

        #endregion

        #region Events

        public event EventHandler Completed;
        public event EventHandler<PlayerErrorEventArgs> Error;
        public event EventHandler Prepared;

        #endregion

        public VideoPlayer()
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height=50},
                new RowDefinition { Height=new GridLength(1,GridUnitType.Star)},
            };

            if (Device.RuntimePlatform == Device.Android)
            {
                imgFullScreen = new Image()
                {
                    HeightRequest = 25,
                    WidthRequest = 25,
                    Margin = 5,
                    IsVisible = false,
                    HorizontalOptions = LayoutOptions.End,
                    Source = "portrait_mode.png",
                };
                imgFullScreen.OnTapped(() =>
                {
                    isPortrait = !isPortrait;
                    _nativePlayer.SetScreen(isPortrait);
                    imgFullScreen.Source = isPortrait ? "landscape_mode.png" : "portrait_mode.png";
                });

                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    imgFullScreen.IsVisible = _nativePlayer.IsNativeControlsVisible;
                    return true;
                });
                Children.Add(imgFullScreen, 0, 0);
            }
        }

        #region Methods

        public void Pause()
        {
            _nativePlayer?.Pause();
        }

        public void Play()
        {
            _nativePlayer?.Play();
        }

        public void Stop()
        {
            _nativePlayer?.Stop();
        }

        public void Seek(int seconds)
        {
            _nativePlayer?.Seek(seconds);
        }

        #endregion

        #region Internal Methods

        internal void SetNativeContext(INativePlayer player)
        {
            _nativePlayer = player;
        }

        internal void OnError(string error)
        {
            Error?.Invoke(this, new PlayerErrorEventArgs(error));
        }

        internal void OnCompletion()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        internal void OnPrepare()
        {
            Prepared?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
