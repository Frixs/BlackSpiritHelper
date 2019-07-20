using System;
using System.Diagnostics;
using System.Windows.Media;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper for the <see cref="MediaPlayer"/>.
    /// TODO: Rework.
    /// </summary>
    public class AudioPlayer
    {
        #region Private Members

        private MediaPlayer mMediaPlayer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Position of the player.
        /// </summary>
        public TimeSpan Position
        {
            get
            {
                return mMediaPlayer.Position;
            }
            set
            {
                mMediaPlayer.Position = value;
            }
        }

        /// <summary>
        /// Says, if the player is currently playing any media.
        /// </summary>
        public bool IsPlaying { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AudioPlayer()
        {
            mMediaPlayer = new MediaPlayer();
            
            // Start events.
            StartEvents();
        }

        #endregion

        #region Events

        /// <summary>
        /// Start events.
        /// </summary>
        private void StartEvents()
        {
            // On media end.
            mMediaPlayer.MediaEnded += (o, args) =>
            {
                IsPlaying = false;
            };
            // On media failed.
            mMediaPlayer.MediaFailed += (o, args) =>
            {
                IoC.Logger.Log($"Failed to open audio file: {mMediaPlayer.Source}", LogLevel.Error);
                Debugger.Break();
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Open (<see cref="Open(Uri)"/>) and Play (<see cref="Play"/>).
        /// </summary>
        /// <param name="source"></param>
        public void OpenAndPlay(Uri source)
        {
            mMediaPlayer.Dispatcher.BeginInvoke((Action)(() =>
            {
                Open(source);
                Play();
            }));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Opens the given <see cref="Uri"/> for the given media playback.
        /// </summary>
        /// <param name="source"></param>
        private void Open(Uri source)
        {
            mMediaPlayer.Open(source);
        }

        /// <summary>
        /// Play media.
        /// </summary>
        private void Play()
        {
            // Reset position.
            Stop();

            IsPlaying = true;
            mMediaPlayer.Play();
        }

        /// <summary>
        /// Stop media playback.
        /// </summary>
        private void Stop()
        {
            mMediaPlayer.Stop();
            IsPlaying = false;
        }

        #endregion
    }
}
