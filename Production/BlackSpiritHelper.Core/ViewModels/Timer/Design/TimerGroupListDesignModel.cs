namespace BlackSpiritHelper.Core
{
    public class TimerGroupListDesignModel : TimerGroupListViewModel
    {
        #region Singleton

        public static TimerGroupListDesignModel Instance => new TimerGroupListDesignModel();

        #endregion

        #region Constructor

        public TimerGroupListDesignModel()
        {
            AddGroup("Default");
            AddGroup("Test Group");
        }

        #endregion
    }
}
