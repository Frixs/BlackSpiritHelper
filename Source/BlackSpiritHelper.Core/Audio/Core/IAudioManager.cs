using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    public interface IAudioManager
    {
        /// <summary>
        /// Play audio according to type and priority.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        Task PlayAsync(AudioType type, AudioPriorityBracket priority);

        /// <summary>
        /// Get the desired audio from the manager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AudioFile GetAudio(AudioType type);
    }
}
