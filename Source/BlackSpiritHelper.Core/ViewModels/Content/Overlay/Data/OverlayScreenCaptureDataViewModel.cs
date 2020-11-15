namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Overlay screen capture data
    /// </summary>
    public class OverlayScreenCaptureDataViewModel
    {
        /// <summary>
        /// X axis position of the overlay object.
        /// </summary>
        public float PosX { get; set; } = 0;

        /// <summary>
        /// Y axis position of the overlay object.
        /// </summary>
        public float PosY { get; set; } = 0;

        // TODO ---
        public float Scale { get; set; } = 1;

        public float Opacity { get; set; } = 1;
    }
}
