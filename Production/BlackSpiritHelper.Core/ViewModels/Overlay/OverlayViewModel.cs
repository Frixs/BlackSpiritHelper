using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class OverlayViewModel : DataContentBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// X axis position of the overlay object.
        /// </summary>
        public double PosX { get; set; } = 0;

        /// <summary>
        /// Y axis position of the overlay object.
        /// </summary>
        public double PosY { get; set; } = 0;

        /// <summary>
        /// Says, if the dragging with overlay object is locked or not.
        /// </summary>
        public bool IsDraggingLocked { get; set; } = false;

        /// <summary>
        /// Overlay orientation.
        /// </summary>
        public Orientation OverlayOrientation { get; set; } = Orientation.Horizontal;

        /// <summary>
        /// List of all types of <see cref="Orientation"/>.
        /// </summary>
        [XmlIgnore]
        public Orientation[] OrientationList { get; private set; } = (Orientation[])Enum.GetValues(typeof(Orientation));

        /// <summary>
        /// Overlay size.
        /// <see cref="OverlaySize"/> handles the sizes in pixels.
        /// </summary>
        public OverlaySize OverlaySize { get; set; } = OverlaySize.Normal;

        /// <summary>
        /// List of all types of <see cref="OverlaySize"/>.
        /// </summary>
        [XmlIgnore]
        public OverlaySize[] OverlaySizeList { get; private set; } = (OverlaySize[])Enum.GetValues(typeof(OverlaySize));

        /// <summary>
        /// Get pixel size string for style application.
        /// </summary>
        [XmlIgnore]
        public double OverlaySizeStyleValue => (double)OverlaySize;

        /// <summary>
        /// Get pixel size string for Width style application.
        /// </summary>
        [XmlIgnore]
        public double OverlaySizeStyleWidthValue => GetUserPreferredOverlaySizeWidthStyleValue();

        /// <summary>
        /// Get pixel size string for Height style application.
        /// </summary>
        [XmlIgnore]
        public double OverlaySizeStyleHeightValue => GetUserPreferredOverlaySizeHeightStyleValue();

        public override bool IsRunning => throw new NotImplementedException();

        #endregion

        #region Commands

        /// <summary>
        /// The command to lock overlay dragging.
        /// </summary>
        [XmlIgnore]
        public ICommand LockOverlayDraggingCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OverlayViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        /// <summary>
        /// Everythng you need to do after construction.
        /// </summary>
        protected override void SetupMethod()
        {
        }

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        protected override void SetDefaultsMethod()
        {
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            LockOverlayDraggingCommand = new RelayCommand(() => LockOverlay());
        }

        /// <summary>
        /// Lock/Unlock Overlay dragging.
        /// </summary>
        private void LockOverlay()
        {
            IsDraggingLocked = !IsDraggingLocked;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Transform size according to orientation into Width style value.
        /// </summary>
        /// <returns></returns>
        private double GetUserPreferredOverlaySizeWidthStyleValue()
        {
            if (OverlayOrientation == Orientation.Vertical)
                return (double)OverlaySize;

            return double.NaN; // Auto.
        }

        /// <summary>
        /// Transform size according to orientation into Height style value.
        /// </summary>
        /// <returns></returns>
        private double GetUserPreferredOverlaySizeHeightStyleValue()
        {
            if (OverlayOrientation == Orientation.Horizontal)
                return (double)OverlaySize;

            return double.NaN; // Auto.
        }

        #endregion
    }
}
