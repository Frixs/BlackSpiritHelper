using System;

namespace BlackSpiritHelper.Core
{
    public sealed class AudioFile
    {
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
    }
}
