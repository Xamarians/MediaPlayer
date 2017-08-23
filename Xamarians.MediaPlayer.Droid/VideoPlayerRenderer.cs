using Android.App;
using Android.Content;
using Android.Media;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using System;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarians.MediaPlayer.VideoPlayer), typeof(Xamarians.MediaPlayer.Droid.VideoPlayerRenderer))]
namespace Xamarians.MediaPlayer.Droid
{

    class MyMediaController : MediaController
    {
        public event System.EventHandler<bool> VisibilityChange;
        public MyMediaController(Context context, bool useFastForward)
            : base(context, useFastForward)
        {

        }

        public override void Show()
        {
            base.Show();
            VisibilityChange?.Invoke(this, true);
        }

        public override void Hide()
        {
            base.Hide();
            VisibilityChange?.Invoke(this, false);
        }
    }

    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, RelativeLayout>, INativePlayer
    {
        VideoView _videoView;
        ProgressBar progressBar;
        MyMediaController mediaController;
        bool _prepared;
        static Activity _context;
        double playerHeight;
        bool hasScrollView = false;
        const string FullScreenImageSource = "landscape_mode.png";
        const string ExitFullScreenImageSource = "portrait_mode.png";
        ImageView imageView;

        static double deviceWidth;
        static double deviceHeight;
        //System.Timers.Timer timer;
        public event EventHandler<bool> FullScreenStatusChanged;

        public static void Init(Activity context)
        {
            _context = context;
            deviceWidth = (int)(context.Resources.DisplayMetrics.WidthPixels / context.Resources.DisplayMetrics.Density);
            deviceHeight = (int)(context.Resources.DisplayMetrics.HeightPixels / context.Resources.DisplayMetrics.Density);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                return;
            var context = Xamarin.Forms.Forms.Context;

            // Set Native Control
            var relativeLayout = new RelativeLayout(context);
            relativeLayout.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            relativeLayout.SetPadding(0, 0, 0, 0);
            relativeLayout.SetBackgroundColor(Android.Graphics.Color.Black);
            SetNativeControl(relativeLayout);
            Element.SetNativeContext(this);

            // Create Video View
            InitVideoView();

            // Start the MediaController
            InitMediaController();

            // Show progressbar
            InitProgressBar();
            SetSource();

            InitializeFullScreenCapability();

        }

        private void InitializeFullScreenCapability()
        {
            Xamarin.Forms.Element view = Element;
            while (view.Parent != null)
            {
                view = view.Parent;
                if (view is Xamarin.Forms.ScrollView)
                {
                    (view as Xamarin.Forms.ScrollView).Scrolled += delegate { DisplaySeekbar(false); };
                    hasScrollView = true;
                    break;
                }
            }
            imageView = new ImageView(_context) { };
            imageView.SetImageResource(Resource.Drawable.portrait_mode);
            var lv = new RelativeLayout.LayoutParams(60, 60);
            lv.SetMargins(0, 30, 30, 0);
            lv.AddRule(LayoutRules.AlignParentRight);
            imageView.LayoutParameters = lv;
            Control.AddView(imageView);

            imageView.Click += delegate
            {
                if (IsFullScreen)
                    ExitFullScreen();
                else
                    FullScreen(hasScrollView);
            };
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (VideoPlayer.SourceProperty.PropertyName.Equals(e.PropertyName))
            {
                SetSource();
            }
        }


        #region Private Methods

        private void InitVideoView()
        {
            _videoView = new VideoView(Context);
            _videoView.Holder.SetKeepScreenOn(true);
            _videoView.Prepared += videoView_Prepared;
            _videoView.Error += videoView_Error;
            _videoView.Completion += videoView_Completion;
            _videoView.Info += videoView_Info;

            var lv = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            lv.AddRule(LayoutRules.CenterInParent);
            _videoView.LayoutParameters = lv;
            Control.AddView(_videoView);
        }

        private void InitMediaController()
        {
            mediaController = new MyMediaController(Context, false);
            mediaController.VisibilityChange += MediaController_VisibilityChange;
            mediaController.SetAnchorView(_videoView);
            _videoView.SetMediaController(mediaController);
        }

        private void MediaController_VisibilityChange(object sender, bool value)
        {
            imageView.Visibility = value ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible; ;
        }

        private void InitProgressBar()
        {
            progressBar = new ProgressBar(Context);
            progressBar.Indeterminate = false;
            progressBar.Visibility = Android.Views.ViewStates.Invisible;
            var lparams = new RelativeLayout.LayoutParams(100, 100);
            lparams.AddRule(LayoutRules.CenterInParent);
            Control.AddView(progressBar, lparams);
        }

        private void SetSource()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Element.Source))
                    return;
                _prepared = false;
                progressBar.Visibility = Android.Views.ViewStates.Visible;
                _videoView.SetVideoURI(Android.Net.Uri.Parse(Element.Source));
                _videoView.RequestFocus();
            }
            catch (Java.Lang.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                Element.OnError(e.Message);
            }
        }

        #endregion

        #region INativePlayer

        bool isFullScreen = false;


        private bool IsFullScreen
        {
            get
            {
                return isFullScreen;
            }
        }

        public bool IsSeekbarVisible
        {
            get
            {
                return mediaController?.IsShown ?? false;
            }
        }



        public int Duration
        {
            get
            {
                return _prepared ? _videoView.Duration : 0;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return _prepared ? _videoView.CurrentPosition : 0;
            }
        }

        public void Play()
        {
            if (!_prepared) return;
            _videoView.Start();
        }

        public void Pause()
        {
            if (!_prepared) return;
            if (_videoView.CanPause())
                _videoView.Pause();
        }

        public void Stop()
        {
            if (!_prepared) return;
            _videoView.StopPlayback();
        }

        public void Seek(int seconds)
        {
            if (!_prepared) return;
            _videoView.SeekTo(seconds * 1000);
        }

        /// <summary>
        /// Change screen orientation to Landscape and set video player in full screen mode.
        /// </summary>
        /// <param name="resizeLayout">set it True if you are using video player inside a scroo view</param>
        private void FullScreen(bool resizeLayout = false)
        {
            if (isFullScreen)
                return;

            _context.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            var window = (_context as Activity).Window;
            window.AddFlags(WindowManagerFlags.Fullscreen);
            imageView.SetImageResource(Resource.Drawable.landscape_mode);
            isFullScreen = true;
            if (resizeLayout)
            {
                if (Element.HeightRequest == -1)
                    playerHeight = (Control.Width / _context.Resources.DisplayMetrics.Density);
                else
                    playerHeight = Element.HeightRequest;
                Element.HeightRequest = deviceWidth;
            }
            else
            {
                playerHeight = 0;
            }
            FullScreenStatusChanged?.Invoke(this, true);
        }

        private void ExitFullScreen()
        {
            if (!isFullScreen)
                return;

            imageView.SetImageResource(Resource.Drawable.portrait_mode);
            _context.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            var window = (_context as Activity).Window;
            window.ClearFlags(WindowManagerFlags.Fullscreen);
            isFullScreen = false;
            if (playerHeight > 0)
            {
                Element.HeightRequest = playerHeight;
            }
            FullScreenStatusChanged?.Invoke(this, false);
        }

        public void DisplaySeekbar(bool value)
        {
            if (mediaController == null)
                return;
            if (value)
                mediaController.Show();
            else
                mediaController.Hide();
        }

        #endregion

        #region Events

        private void videoView_Prepared(object sender, System.EventArgs e)
        {
            progressBar.Visibility = Android.Views.ViewStates.Invisible;
            _prepared = true;
            if (Element.AutoPlay)
                Play();
            Element?.OnPrepare();
        }

        private void videoView_Info(object sender, Android.Media.MediaPlayer.InfoEventArgs e)
        {
            progressBar.Visibility = e.What == MediaInfo.BufferingStart ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;
        }

        private void videoView_Completion(object sender, System.EventArgs e)
        {
            Element?.OnCompletion();
        }

        private void videoView_Error(object sender, Android.Media.MediaPlayer.ErrorEventArgs e)
        {
            Element?.OnError(e.What.ToString());
        }

        #endregion

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!IsFullScreen)
            {
                return base.OnTouchEvent(e);
            }
            if (e.Action == MotionEventActions.Down)
            {
                Parent.RequestDisallowInterceptTouchEvent(true);
            }
            if (e.Action == MotionEventActions.Up)
            {
                Parent.RequestDisallowInterceptTouchEvent(false);
            }
            return true;
        }

    }
}