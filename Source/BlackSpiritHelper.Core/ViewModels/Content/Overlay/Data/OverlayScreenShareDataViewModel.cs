namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// TODO
    /// </summary>
    public class OverlayScreenShareDataViewModel
    {
        /// <summary>
        /// X axis position of the overlay object.
        /// </summary>
        public double PosX { get; set; } = 0;

        /// <summary>
        /// Y axis position of the overlay object.
        /// </summary>
        public double PosY { get; set; } = 0;

        public float Scale { get; set; } = 1;

        public float Opacity { get; set; } = 1;
    }
}
