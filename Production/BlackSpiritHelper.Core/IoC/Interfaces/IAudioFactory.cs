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
        void Play(AudioType type);
    }
}
