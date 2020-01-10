using System.Collections.Generic;
using System.Linq;

namespace BlackSpiritHelper.Core
{
    public class NotificationAreaDialogViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// List of oall notifications ready to be shown to a user
        /// </summary>
        private List<NotificationBoxDialogViewModel> mNotificationList = new List<NotificationBoxDialogViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Next notification in row to be shown to a user.
        /// </summary>
        public NotificationBoxDialogViewModel NextNotification => mNotificationList.Count > 0 ? mNotificationList[0] : null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NotificationAreaDialogViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new notification to the list.
        /// </summary>
        /// <param name="viewModel"></param>
        public void AddNotification(NotificationBoxDialogViewModel viewModel)
        {
            if (viewModel == null)
                return;

            mNotificationList.Add(viewModel);
            OnPropertyChanged(nameof(NextNotification));
        }

        /// <summary>
        /// Remove notification from the list.
        /// </summary>
        /// <param name="viewModel"></param>
        public void RemoveNotification(NotificationBoxDialogViewModel viewModel)
        {
            if (viewModel == null)
                return;

            mNotificationList.Remove(viewModel);
            OnPropertyChanged(nameof(NextNotification));
        }

        #endregion
    }
}
