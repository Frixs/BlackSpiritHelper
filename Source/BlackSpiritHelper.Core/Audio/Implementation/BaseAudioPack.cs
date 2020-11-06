using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Base logic for audi packs
    /// </summary>
    public abstract class BaseAudioPack : IAudioPack
    {
        #region Private Members

        /// <summary>
        /// Audio player of the manager
        /// </summary>
        private readonly AudioPlayer mAudioPlayer = new AudioPlayer();

        #endregion

        #region Protected Members

        /// <summary>
        /// Audio sample list of the audio pack
        /// </summary>
        protected Dictionary<AudioSampleType, AudioSample> mAudioSamples = new Dictionary<AudioSampleType, AudioSample>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseAudioPack()
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Play(AudioSampleType type, AudioPriorityBracket priority)
        {
            // Play audio if it is in the correct priority bracket...
            if (priority == AudioPriorityBracket.Pack && !mAudioPlayer.IsPlaying)
            {
                var audioFile = GetAudioSampleFile(type);
                if (audioFile != null)
                {
                    // Play the pack manager audio
                    mAudioPlayer.Play(audioFile.URI);
                }
            }
            // Force.
            else if (priority == AudioPriorityBracket.PackForce)
            {
                // Stop all playing the current audio first...
                foreach (KeyValuePair<AudioSampleType, AudioSample> audio in mAudioSamples)
                    audio.Value.Stop();

                var audioFile = GetAudioSampleFile(type);
                if (audioFile != null)
                {
                    // Play the pack manager priority audio
                    mAudioPlayer.Play(audioFile.URI);
                }
            }
            // Otherwise play lower bracket audio, if the pack manager is not playing atm...
            else if (!mAudioPlayer.IsPlaying)
            {
                // Play on sample bracket
                GetAudioSample(type)?.Play();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Add new audio sample to the list.
        /// </summary>
        /// <param name="item">The sample</param>
        protected void AddAudioSample(AudioSample item)
        {
            if (item == null)
            {
                // Log it.
                IoC.Logger.Log("Invalid audio sample!", LogLevel.Error);
                Debugger.Break();
                return;
            }

            mAudioSamples.Add(item.Type, item);
        }

        /// <summary>
        /// Add new audio sample to the list with possibility to add it with initial configuration.
        /// <see cref="AudioSample"/>.
        /// </summary>
        /// <param name="type">The type of the new sample</param>
        /// <param name="item">The func to create audio sample</param>
        protected void AddAudioSample(AudioSampleType type, Func<AudioSample, AudioSample> item)
        {
            AddAudioSample(item(new AudioSample(type)));
        }

        /// <summary>
        /// Get the desired audio from the pack by the type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Audio file or null if it does not exist</returns>
        protected AudioFile GetAudioSampleFile(AudioSampleType type)
        {
            var audioSample = GetAudioSample(type);
            if (audioSample == null)
                return null;

            var audioFile = audioSample.GetAudioFile();
            if (audioFile == null)
                return null;

            return audioFile;
        }

        /// <summary>
        /// Get <see cref="AudioSample"/> from <see cref="mAudioSamples"/> by <see cref="AudioSampleType"/>
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Audio sample or null</returns>
        protected AudioSample GetAudioSample(AudioSampleType type)
        {
            if (mAudioSamples.ContainsKey(type))
                return mAudioSamples[type];

            // Log it.
            IoC.Logger.Log($"Audio sample '{type}' does not exist!", LogLevel.Error);
            return null;
        }

        #endregion
    }
}
