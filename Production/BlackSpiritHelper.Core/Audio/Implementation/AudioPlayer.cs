using System;
using System.Windows.Media;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper for the <see cref="MediaPlayer"/>.
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
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the given <see cref="Uri"/> for the given media playback.
        /// </summary>
        /// <param name="source"></param>
        public void Open(Uri source)
        {
            mMediaPlayer.Open(source);
        }

        /// <summary>
        /// Open (<see cref="Open(Uri)"/>) and Play (<see cref="Play"/>).
        /// </summary>
        /// <param name="source"></param>
        public void OpenAndPlay(Uri source)
        {
            Open(source);
            Play();
        }

        /// <summary>
        /// Play media.
        /// </summary>
        public void Play()
        {
            Position = TimeSpan.Zero;
            mMediaPlayer.Play();
        }

        /// <summary>
        /// Play media from the current <see cref="Position"/>.
        /// </summary>
        public void Continue()
        {
            mMediaPlayer.Play();
        }

        /// <summary>
        /// Pause media playback.
        /// </summary>
        public void Pause()
        {
            mMediaPlayer.Pause();
        }

        /// <summary>
        /// Stop media playback.
        /// </summary>
        public void Stop()
        {
            mMediaPlayer.Stop();
        }

        #endregion
    }
}
