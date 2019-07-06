namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuDesignModel : TimerGroupMenuViewModel
    {
        #region Singleton

        public static TimerGroupMenuDesignModel Instance => new TimerGroupMenuDesignModel();

        #endregion

        #region Constructor

        public TimerGroupMenuDesignModel()
        {
            AddGroup("Default");
            AddGroup("Test Group");
        }

        #endregion
    }
}
