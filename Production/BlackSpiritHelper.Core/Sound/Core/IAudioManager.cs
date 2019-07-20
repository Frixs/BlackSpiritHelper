using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public interface IAudioManager
    {
        /// <summary>
        /// Get the desired audio from the manager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AudioFile GetAudio(AudioType type);
    }
}
