using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Base audio factory handling the whole app audio system
    /// </summary>
    public class BaseAudioFactory : IAudioFactory
    {
        #region Protected Members

        /// <summary>
        /// The list of <see cref="IAudioPack"/> in this factory.
        /// </summary>
        protected Dictionary<AudioAlertType, IAudioPack> mAudioPacks = new Dictionary<AudioAlertType, IAudioPack>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseAudioFactory()
        {
            mAudioPacks.Add(AudioAlertType.Standard, new StandardAudioPack());
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Play(AudioSampleType type, AudioPriorityBracket priority = AudioPriorityBracket.Sample)
        {
            // We do not want to play any audio.
            if (IoC.DataContent.PreferencesData.AudioAlertType == AudioAlertType.None)
                return;

            switch (IoC.DataContent.PreferencesData.AudioAlertType)
            {
                // Standard.
                case AudioAlertType.Standard:
                    mAudioPacks[AudioAlertType.Standard].Play(type, priority);
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
