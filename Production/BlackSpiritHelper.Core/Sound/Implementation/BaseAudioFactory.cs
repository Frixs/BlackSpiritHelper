﻿using System.Collections.Generic;
using System.Windows.Media;

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
        /// Play the audio according to type.
        /// </summary>
        /// <param name="type"></param>
        public void Play(AudioType type)
        {
            // We do not want to play any audio.
            if (IoC.DataContent.PreferencesDesignModel.AudioAlertLevel == AudioAlertLevel.None)
                return;

            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(mAudioManagers[IoC.DataContent.PreferencesDesignModel.AudioAlertLevel].GetAudio(type).URI);
        }

        #endregion
    }
}
