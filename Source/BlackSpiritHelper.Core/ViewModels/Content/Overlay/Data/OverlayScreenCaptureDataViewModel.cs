namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Overlay screen capture data
    /// </summary>
    public class OverlayScreenCaptureDataViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        public static float LimitScale_Max => 1f;
        public static float LimitScale_Min => 0.1f;

        public static float LimitOpacity_Max => 1f;
        public static float LimitOpacity_Min => 0.3f;

        #endregion

        /// <summary>
        /// X axis position of the overlay object.
        /// </summary>
        public float PosX { get; set; } = 0;

        /// <summary>
        /// Y axis position of the overlay object.
        /// </summary>
        public float PosY { get; set; } = 0;

        /// <summary>
        /// Scale of the capture surface
        /// </summary>
        public float Scale { get; set; } = 0.4f;

        /// <summary>
        /// Opacity of the capture surface
        /// </summary>
        public float Opacity { get; set; } = 0.95f;
    }
}
