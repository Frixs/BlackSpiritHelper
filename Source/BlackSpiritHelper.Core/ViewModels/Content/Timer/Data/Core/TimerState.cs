namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Timer states.
    /// </summary>
    public enum TimerState
    {
        /// <summary>
        /// Default value to let timer know to set parameters.
        /// </summary>
        None = 0,

        /// <summary>
        /// Timer is ready to start with countdown.
        /// </summary>
        Ready = 1,

        /// <summary>
        /// Timer is playing.
        /// </summary>
        Play = 2,

        /// <summary>
        /// Timer is stopped.
        /// </summary>
        Pause = 3,

        /// <summary>
        /// Timer is stopped by group control.
        /// </summary>
        Freeze = 4,

        /// <summary>
        /// Timer is in countdown phase.
        /// </summary>
        Countdown = 5,
    }
}
