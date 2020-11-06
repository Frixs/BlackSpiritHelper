using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper for the <see cref="MediaPlayer"/>.
    /// TODO:LATER: Rework Custom Audio Player - not important atm.
    /// </summary>
    public sealed class AudioPlayer
    {
        #region Private Members

        private readonly MediaPlayer mMediaPlayer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says, if the player is currently playing any media.
        /// </summary>
        public bool IsPlaying { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
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
                IoC.Logger.Log($"Media failed. Source: '{mMediaPlayer.Source}'", LogLevel.Error);
                Debugger.Break();
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Open (<see cref="OpenAudio(Uri)"/>) and Play (<see cref="Play"/>).
        /// </summary>
        /// <param name="source"></param>
        public void Play(Uri source)
        {
            if (source == null || source.ToString().Length == 0)
            {
                IoC.Logger.Log($"Invalid audio URI! Audio will not be played!", LogLevel.Error);
                return;
            }

            IoC.Dispatcher.BeginInvoke(mMediaPlayer.Dispatcher, (Action)(() =>
            {
                // Reset position.
                mMediaPlayer.Stop();

                OpenAudio(source);
                StartPlay();
            }));
        }

        /// <summary>
        /// Stop media playback.
        /// </summary>
        public void Stop()
        {
            IoC.Dispatcher.BeginInvoke(mMediaPlayer.Dispatcher, (Action)(() =>
            {
                mMediaPlayer.Stop();
                IsPlaying = false;
            }));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Opens the given <see cref="Uri"/> for the given media playback.
        /// </summary>
        /// <param name="source"></param>
        private void OpenAudio(Uri source)
        {
            mMediaPlayer.Open(source);
        }

        /// <summary>
        /// Play media.
        /// </summary>
        private void StartPlay()
        {
            IsPlaying = true;
            mMediaPlayer.Volume = IoC.DataContent.PreferencesData.Volume;
            mMediaPlayer.Play();
        }

        #endregion
    }
}
