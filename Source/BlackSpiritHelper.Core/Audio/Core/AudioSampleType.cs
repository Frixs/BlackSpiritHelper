namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Audio Type.
    /// </summary>
    public enum AudioSampleType
    {
        /// <summary>
        /// Default value. /No value.
        /// </summary>
        None = 0,

        /// <summary>
        /// Timer alert Lv3.
        /// </summary>
        Alert3 = 1,

        /// <summary>
        /// Timer alert Lv2.
        /// </summary>
        Alert2 = 2,

        /// <summary>
        /// Timer alert Lv1.
        /// </summary>
        Alert1 = 3,

        /// <summary>
        /// Alert of type 4.
        /// </summary>
        Alert4 = 4,

        /// <summary>
        /// Alert of type 5.
        /// </summary>
        Alert5 = 5,

        /// <summary>
        /// Timer alert counting.
        /// </summary>
        AlertCountdown = 6,

        /// <summary>
        /// Alert user a lot of time before the event start.
        /// </summary>
        AlertLongBefore = 7,

        /// <summary>
        /// Alert of time ticking.
        /// </summary>
        AlertClockTicking = 8,

        /// <summary>
        /// Start notification #1
        /// </summary>
        StartNotification1 = 9,

        /// <summary>
        /// Stop notification #1
        /// </summary>
        StopNotification1 = 10,

        /// <summary>
        /// Ping notification #1
        /// </summary>
        PingNotification1 = 11,
    }
}
