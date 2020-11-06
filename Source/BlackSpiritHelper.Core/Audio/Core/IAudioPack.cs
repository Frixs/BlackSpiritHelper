namespace BlackSpiritHelper.Core
{
    public interface IAudioPack
    {
        /// <summary>
        /// Play audio according to type and priority
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="priority">The priority</param>
        void Play(AudioSampleType type, AudioPriorityBracket priority);
    }
}
