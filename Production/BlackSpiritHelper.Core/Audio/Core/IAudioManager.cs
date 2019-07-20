using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public interface IAudioManager
    {
        /// <summary>
        /// Play audio according to type and priority.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        void Play(AudioType type, AudioPriorityBracket priority);

        /// <summary>
        /// Get the desired audio from the manager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AudioFile GetAudio(AudioType type);
    }
}
