using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Base Overlay wrapper data view model
    /// </summary>
    public class OverlayBaseDataViewModel : BaseViewModel
    {
        /// <summary>
        /// X axis position of the overlay object.
        /// </summary>
        public double PosX { get; set; } = 0;

        /// <summary>
        /// Y axis position of the overlay object.
        /// </summary>
        public double PosY { get; set; } = 0;

        /// <summary>
        /// Overlay orientation.
        /// </summary>
        public Orientation OverlayOrientation { get; set; } = Orientation.Horizontal;

        /// <summary>
        /// Overlay size
        /// </summary>
        public BaseOverlaySize Size { get; set; } = BaseOverlaySize.Normal;

        /// <summary>
        /// Get pixel size string for style application
        /// </summary>
        public double SizeValue => (double)Size;

        /// <summary>
        /// Get pixel size string for Width style application
        /// </summary>
        public double SizeWidthValue => OverlayOrientation == Orientation.Vertical ? SizeValue : double.NaN;

        /// <summary>
        /// Get pixel size string for Height style application
        /// </summary>
        public double SizeHeightValue => OverlayOrientation == Orientation.Horizontal ? SizeValue : double.NaN;
    }
}
