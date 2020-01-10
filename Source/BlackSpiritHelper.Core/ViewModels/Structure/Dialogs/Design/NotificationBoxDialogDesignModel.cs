namespace BlackSpiritHelper.Core
{
    public class NotificationBoxDialogDesignModel : NotificationBoxDialogViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public static NotificationBoxDialogDesignModel Instance => new NotificationBoxDialogDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NotificationBoxDialogDesignModel()
        {
            Title = "NOTIFICATION";
            Message = "Message placeholder!";
        }

        #endregion
    }
}
