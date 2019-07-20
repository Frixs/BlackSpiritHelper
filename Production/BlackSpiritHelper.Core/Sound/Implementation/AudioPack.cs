using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    public class AudioPack
    {
        #region Private Members

        /// <summary>
        /// List of all audio files in this pack.
        /// </summary>
        private List<AudioFile> mList;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AudioPack()
        {
            mList = new List<AudioFile>();
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Get random audio from the pack.
        /// </summary>
        public AudioFile GetAudio()
        {
            // List has no items.
            if (mList.Count <= 0)
                return null;

            // List has only 1 item.
            if (mList.Count == 1)
                return mList[0];

            // List has multiple items.
            Random rnd = new Random();
            int r = rnd.Next(mList.Count);
            return mList[r];
        }

        /// <summary>
        /// Add file into the pack.
        /// </summary>
        /// <param name="file"></param>
        public void AddAudio(AudioFile file)
        {
            if (file == null)
            {
                // Log it.
                IoC.Logger.Log("Invalid audio file!", LogLevel.Error);
                Debugger.Break();
                return;
            }

            mList.Add(file);
        }

        #endregion
    }
}
