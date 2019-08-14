using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    public abstract class BaseAudioManager : IAudioManager
    {
        #region Private Members

        /// <summary>
        /// Audio player of the manager.
        /// </summary>
        private AudioPlayer mAudioPlayer = new AudioPlayer();

        #endregion

        #region Protected Members

        /// <summary>
        /// Audio List.
        /// </summary>
        protected Dictionary<AudioType, AudioPack> mAudioList = new Dictionary<AudioType, AudioPack>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseAudioManager()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play audio according to type and priority.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        public async Task PlayAsync(AudioType type, AudioPriorityBracket priority)
        {
            // Check if media player is not busy.
            if (mAudioPlayer.IsPlaying)
                return;

            // Continue in another thread.
            await Task.Delay(1);

            // Play audio if it is in the correct priority bracket.
            if (priority == AudioPriorityBracket.Manager)
            {
                mAudioPlayer.OpenAndPlay(GetAudio(type).URI);
                return;
            }
            // Force.
            else if (priority == AudioPriorityBracket.ManagerForce)
            {
                mAudioPlayer.OpenAndPlay(GetAudio(type).URI);
                // Stop all playing audio.
                foreach (
                    AudioType at in (AudioType[])Enum.GetValues(typeof(AudioType))
                    )
                    mAudioList[at].Stop();

                return;
            }

            // Play lower bracket.
            GetAudioPack(type).Play(priority);
        }

        /// <summary>
        /// Get the desired audio from the manager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AudioFile GetAudio(AudioType type)
        {
            var audio = GetAudioPack(type).GetAudio();
            if (audio == null)
            {
                // Log it.
                IoC.Logger.Log($"Cannot find any audio in audio pack '{type.ToString()}'!", LogLevel.Error);
                return null;
            }

            return audio;
        }

        /// <summary>
        /// Add new audio pack to the list.
        /// <see cref="AudioPack"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        public void AddPack(AudioType type, AudioPack item)
        {
            if (item == null)
            {
                // Log it.
                IoC.Logger.Log("Invalid audio pack!", LogLevel.Error);
                Debugger.Break();
                return;
            }

            // Set type of the pack.
            item.Type = type;

            mAudioList.Add(type, item);
        }

        /// <summary>
        /// Add new audio pack to the list with possibility to add it with initial configuration.
        /// <see cref="AudioPack"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        public void AddPack(AudioType type, Func<AudioPack, AudioPack> item)
        {
            AddPack(type, item(new AudioPack()));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get <see cref="AudioPack"/> from <see cref="mAudioList"/> by <see cref="AudioType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private AudioPack GetAudioPack(AudioType type)
        {
            if (!mAudioList.ContainsKey(type))
            {
                // Log it.
                IoC.Logger.Log($"Audio pack '{type.ToString()}' does not exist!", LogLevel.Error);
                return null;
            }

            return mAudioList[type];
        }

        #endregion
    }
}
