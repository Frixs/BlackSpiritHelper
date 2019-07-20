using System;
using System.IO;
using System.Reflection;

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
            //URI = new Uri("Resources/Sounds/" + filePath, UriKind.RelativeOrAbsolute);
            //var outPutDirectory = Path.GetDirectoryName(IoC.Application.ApplicationExecutingAssembly.CodeBase);
            var outPutDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //var mediaPath = Path.Combine(outPutDirectory, "Resources\\Sounds\\" + filePath);
            // TODO;
            var mediaPath = "/BlackSpiritHelper.Core;component/Resources/Sounds/" + filePath;
            URI = new Uri(mediaPath, UriKind.Relative);
            Console.WriteLine("x: " + URI);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play audio according to priority.
        /// </summary>
        /// <param name="priority"></param>
        public void Play(AudioPriorityBracket priority)
        {
            // TODO: Split it into threads. This should be callable everytime.
            // Check if media player is not busy.
            if (mAudioPlayer.IsPlaying)
                return;

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
