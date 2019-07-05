namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Timer states.
    /// </summary>
    public enum TimerState
    {
        /// <summary>
        /// Timer is playing.
        /// </summary>
        Play = 0,

        /// <summary>
        /// Timer is stopped.
        /// </summary>
        Pause = 1,

        /// <summary>
        /// Timer is ready to start with countdown.
        /// </summary>
        Ready = 2,

        /// <summary>
        /// Timer is stopped by group control.
        /// </summary>
        Freeze = 3,
    }
}
