using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    public class SoundAudioManager : IAudioManager
    {
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
        public SoundAudioManager()
        {
            // Pack.
            AddPack(AudioType.TimerAlertLevel3, o => 
            {
                o.AddAudio(new AudioFile("TimerAlert3_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.TimerAlertLevel2, o =>
            {
                o.AddAudio(new AudioFile("TimerAlert2_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.TimerAlertLevel1, o =>
            {
                o.AddAudio(new AudioFile("TimerAlert1_Sound_0.mp3"));
                return o;
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the desired audio from the manager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AudioFile GetAudio(AudioType type)
        {
            return mAudioList[type].GetAudio();
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
    }
}
