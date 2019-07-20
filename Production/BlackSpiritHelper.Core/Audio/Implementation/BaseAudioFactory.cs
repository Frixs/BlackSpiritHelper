using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    public class BaseAudioFactory : IAudioFactory
    {
        #region Protected Members

        /// <summary>
        /// The list of <see cref="IAudioManager"/> in this factory.
        /// </summary>
        protected Dictionary<AudioAlertLevel, IAudioManager> mAudioManagers = new Dictionary<AudioAlertLevel, IAudioManager>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseAudioFactory()
        {
            mAudioManagers.Add(AudioAlertLevel.Sound, new SoundAudioManager());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play the audio according to type and priority.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        public void Play(AudioType type, AudioPriorityBracket priority = AudioPriorityBracket.File)
        {
            // We do not want to play any audio.
            if (IoC.DataContent.PreferencesDesignModel.AudioAlertLevel == AudioAlertLevel.None)
                return;

            switch (IoC.DataContent.PreferencesDesignModel.AudioAlertLevel)
            {
                // Sound.
                case AudioAlertLevel.Sound:
                    mAudioManagers[AudioAlertLevel.Sound].PlayAsync(type, priority);
                    break;

                // Voice.
                case AudioAlertLevel.Voice:
                    // TODO Voice Manager.
                    Debugger.Break();
                    break;

                default:
                    // Log it.
                    IoC.Logger.Log("A selected audio alert level value is out of box!", LogLevel.Error);
                    Debugger.Break();
                    break;
            }
        }

        #endregion
    }
}
