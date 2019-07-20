namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Audio priority brackets.
    /// </summary>
    public enum AudioPriorityBracket
    {
        /// <summary>
        /// Normal priority.
        /// No restriction to call an audio.
        /// </summary>
        File = 0,

        /// <summary>
        /// Normal priority.
        /// Cannot call multiple audio packs of the same type.
        /// </summary>
        Pack = 1,

        /// <summary>
        /// Normal priority.
        /// Only one audio file can be called at the time.
        /// </summary>
        Manager = 2,
    }
}
