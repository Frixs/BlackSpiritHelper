namespace BlackSpiritHelper.Core
{
    public class SideMenuListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Title of the menu item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Enum <see cref="ApplicationPage"/>.
        /// </summary>
        public ApplicationPage PageEnum { get; set; }

        /// <summary>
        /// Says, if any Helper is running in the page.
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region Constructor

        public SideMenuListItemViewModel()
        {

        }

        #endregion
    }
}
