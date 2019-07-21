namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Audio priority brackets.
    /// </summary>
    public enum AudioPriorityBracket
    {
        /// <summary>
        /// Normal priority.
        /// Cannot call multiple audio packs of the same type.
        /// </summary>
        Pack = 0,

        /// <summary>
        /// High priority.
        /// Only one audio file can be called at the time.
        /// </summary>
        Manager = 1,

        /// <summary>
        /// High priority.
        /// Only one audio file can be called at the time.
        /// Stops all sound effects.
        /// </summary>
        ManagerForce = 2,
    }
}
