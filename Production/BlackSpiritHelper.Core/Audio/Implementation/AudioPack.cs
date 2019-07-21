using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    public class AudioPack
    {
        #region Private Members

        /// <summary>
        /// Audio player of the manager.
        /// </summary>
        private AudioPlayer mAudioPlayer = new AudioPlayer();

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
        /// Play audio according to priority.
        /// </summary>
        /// <param name="priority"></param>
        public void Play(AudioPriorityBracket priority)
        {
            // Check if media player is not busy.
            if (mAudioPlayer.IsPlaying)
                return;

            // Play audio if it is in the correct priority bracket.
            if (priority == AudioPriorityBracket.Pack)
            {
                mAudioPlayer.OpenAndPlay(GetAudio().URI);
                return;
            }
        }

        /// <summary>
        /// Stop playing the audio.
        /// </summary>
        public void Stop()
        {
            if (mAudioPlayer.IsPlaying)
                mAudioPlayer.Stop();
        }

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
