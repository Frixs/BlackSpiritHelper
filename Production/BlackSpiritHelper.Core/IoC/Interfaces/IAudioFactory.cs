namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Holds a bunch of sound lists.
    /// </summary>
    public interface IAudioFactory
    {
        /// <summary>
        /// Play the audio according to type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        void Play(AudioType type, AudioPriorityBracket priority = AudioPriorityBracket.Pack);
    }
}
