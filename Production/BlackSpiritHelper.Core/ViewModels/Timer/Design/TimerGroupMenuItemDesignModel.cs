namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuItemDesignModel : TimerGroupMenuItemViewModel
    {
        #region Singleton

        public static TimerGroupMenuItemDesignModel Instance => new TimerGroupMenuItemDesignModel();

        #endregion

        #region Constructor

        public TimerGroupMenuItemDesignModel()
        {
            ID = 0;
            Title = "GroupName";
            IsRunning = false;
        }

        #endregion
    }
}
