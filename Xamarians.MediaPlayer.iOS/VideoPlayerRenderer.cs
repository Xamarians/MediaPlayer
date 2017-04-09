using AVFoundation;
using AVKit;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Foundation;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarians.MediaPlayer.VideoPlayer), typeof(Xamarians.MediaPlayer.iOS.VideoPlayerRenderer))]
namespace Xamarians.MediaPlayer.iOS
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, UIView>, INativePlayer
    {
        AVPlayer _player;
        AVPlayerViewController _playerController;
        bool _prepared;

        public static new void Init()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                return;

            // Set Native Control
            _playerController = new AVPlayerViewController() { View = { Frame = Frame } };
            _playerController.ShowsPlaybackControls = true;
            _playerController.View.Frame = Bounds;
            SetNativeControl(_playerController.View);
            SetNativeControl(this);

            BackgroundColor = UIColor.Black;
            SetSource();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (VideoPlayer.SourceProperty.PropertyName.Equals(e.PropertyName))
            {
                SetSource();
            }
        }


        #region Private Methods

        private void SetSource()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Element.Source))
                    return;
                _prepared = false;
                if (_player != null)
                {
                    //_player.RemoveObserver(this, (Foundation.NSString)"status");
                    //_player.RemoveObserver(this, (Foundation.NSString)"error");
                    _player.Dispose();
                    _player = null;
                }

                _player = new AVPlayer(Foundation.NSUrl.FromString(Element.Source));
                if (_player.Error != null)
                {
                    Element.OnError(_player?.Error?.LocalizedDescription);
                    return;
                }
                _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;

                //_player.AddObserver(this, (Foundation.NSString)"status", Foundation.NSKeyValueObservingOptions.Initial | Foundation.NSKeyValueObservingOptions.New, IntPtr.Zero);
                //_player.AddObserver(this, (Foundation.NSString)"error", Foundation.NSKeyValueObservingOptions.Initial | Foundation.NSKeyValueObservingOptions.New, IntPtr.Zero);

                _playerController.Player = _player;
                if (_playerController.Player?.Error != null)
                {
                    Element.OnError(_playerController?.Player?.Error?.LocalizedDescription);
                }

            }
            catch (Exception e)
            {
                Element.OnError(e.Message);
            }
        }

        #endregion

        #region INativePlayer

        public int Duration
        {
            get
            {
                return _prepared ? (int)_player.CurrentItem.Duration.Seconds : 0;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return _prepared ? (int)_player.CurrentItem.CurrentTime.Seconds : 0;
            }
        }

        public void Play()
        {
            if (!_prepared) return;
            _player.Play();
        }

        public void Pause()
        {
            if (!_prepared) return;
            _player.Pause();
        }

        public void Stop()
        {
            if (!_prepared) return;
            _player.Pause();
        }

        public void Seek(int seconds)
        {
            if (!_prepared) return;
            _player.Seek(CoreMedia.CMTime.FromSeconds(seconds, 0));
        }

        #endregion

        #region Events


        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (Equals(ofObject, _player))
            {
                if (keyPath.Equals((Foundation.NSString)"status"))
                {
                    if (_player.Status == AVPlayerStatus.ReadyToPlay)
                    {
                        _prepared = true;
                        Element.OnPrepare();
                        if (Element.AutoPlay)
                        {
                            Play();
                        }
                    }
                }
                else if (keyPath.Equals((Foundation.NSString)"error") && _player.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine("Error");

                    Element.OnError(_player.Error?.LocalizedDescription ?? "Error loading player");
                }
            }
        }


        #endregion
    }
}
