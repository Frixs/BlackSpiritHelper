using System;

namespace BlackSpiritHelper.Core
{
    public class AudioFile
    {
        #region Private Members

        /// <summary>
        /// Audio player of the manager.
        /// </summary>
        private AudioPlayer mAudioPlayer = new AudioPlayer();

        #endregion

        #region Public Properties

        /// <summary>
        /// Audio File URI.
        /// </summary>
        public Uri URI { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AudioFile(string filePath)
        {
            URI = new Uri("Resources/Sounds/" + filePath, UriKind.Relative);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play audio according to priority.
        /// </summary>
        /// <param name="priority"></param>
        public void Play(AudioPriorityBracket priority)
        {
            // Play audio if it is in the correct priority bracket.
            if (priority == AudioPriorityBracket.File)
            {
                mAudioPlayer.OpenAndPlay(URI);
                return;
            }
        }

        #endregion
    }
}
