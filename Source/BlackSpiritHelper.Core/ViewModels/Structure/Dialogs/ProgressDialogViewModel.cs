namespace BlackSpiritHelper.Core
{
    public class ProgressDialogViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Title text.
        /// </summary>
        public string Title { get; set; } = "CHECKING FOR UPDATES";

        /// <summary>
        /// Subtitle text.
        /// </summary>
        public string Subtitle { get; set; } = "Data";

        /// <summary>
        /// Work on, text.
        /// </summary>
        public string WorkOn { get; set; } = "Working on something...";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProgressDialogViewModel()
        {
        }

        #endregion
    }
}
