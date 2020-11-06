using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Audio sample can handle multiple audio files. 
    /// It is the main part of the audio packs. 
    /// It should contain the same category of audio files like multiple version of the same type of audio to make audio variable.
    /// </summary>
    public sealed class AudioSample
    {
        #region Private Members

        /// <summary>
        /// Audio player of this sample
        /// </summary>
        private readonly AudioPlayer mAudioPlayer = new AudioPlayer();

        /// <summary>
        /// List of all audio files in this sample
        /// </summary>
        private readonly List<AudioFile> mAudioFiles;

        #endregion

        #region Public Properties

        /// <summary>
        /// Type of this audio sample
        /// </summary>
        public AudioSampleType Type { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AudioSample(AudioSampleType type)
        {
            Type = type;
            mAudioFiles = new List<AudioFile>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play audio according to priority.
        /// </summary>
        public void Play()
        {
            // Check if media player is busy...
            if (mAudioPlayer.IsPlaying)
                // Ignore then...
                return;

            var audioFile = GetAudioFile();
            if (audioFile != null)
            {
                // Play the audio
                mAudioPlayer.Play(audioFile.URI);
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
        /// <returns>AudioFile or null</returns>
        public AudioFile GetAudioFile()
        {
            // List has no items.
            if (mAudioFiles.Count <= 0)
            {
                // Log it.
                IoC.Logger.Log($"Cannot find any audio in audio sample '{Type}'!", LogLevel.Error);
                return null;
            }

            // List has only 1 item.
            if (mAudioFiles.Count == 1)
                return mAudioFiles[0];

            // List has multiple items.
            Random rnd = new Random();
            int r = rnd.Next(mAudioFiles.Count);
            return mAudioFiles[r];
        }

        /// <summary>
        /// Add file into the pack.
        /// </summary>
        /// <param name="file"></param>
        public void AddAudioFile(AudioFile file)
        {
            if (file == null)
            {
                // Log it.
                IoC.Logger.Log("Invalid audio file!", LogLevel.Error);
                Debugger.Break();
                return;
            }

            mAudioFiles.Add(file);
        }

        #endregion
    }
}
