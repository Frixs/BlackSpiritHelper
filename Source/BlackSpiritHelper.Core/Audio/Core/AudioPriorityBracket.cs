namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Audio priority brackets.
    /// </summary>
    public enum AudioPriorityBracket
    {
        /// <summary>
        /// Normal priority.
        /// One one audio file can be played at the time in single audio sample of an audio pack (Multiple audio samples can be played in single audio pack at the same time).
        /// </summary>
        Sample = 0,

        /// <summary>
        /// High priority.
        /// Only one audio file can be played at the time in single audio pack.
        /// </summary>
        Pack = 1,

        /// <summary>
        /// High priority.
        /// Only one audio file can be played at the time in single audio pack (can override the currently played one).
        /// </summary>
        PackForce = 2,
    }
}
