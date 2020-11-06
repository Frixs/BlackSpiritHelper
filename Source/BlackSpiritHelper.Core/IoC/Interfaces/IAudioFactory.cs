namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Holds a bunch of sound lists.
    /// </summary>
    public interface IAudioFactory
    {
        /// <summary>
        /// Play the audio according to type and priority
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="priority">The priority</param>
        void Play(AudioSampleType type, AudioPriorityBracket priority = AudioPriorityBracket.Sample);
    }
}
