namespace BlackSpiritHelper.Core
{
    public class TimerGroupDesignModel : TimerGroupViewModel
    {
        #region Singleton

        public static TimerGroupDesignModel Instance => new TimerGroupDesignModel();

        #endregion

        #region Constructor

        public TimerGroupDesignModel()
        {
            ID = 0;
            Title = "GroupName";
            IsRunning = false;
        }

        #endregion
    }
}
