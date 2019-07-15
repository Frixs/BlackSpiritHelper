namespace BlackSpiritHelper.Core
{
    public class TimerGroupDesignModel : TimerGroupViewModel
    {
        #region New Instance Getter

        public static TimerGroupDesignModel Instance => new TimerGroupDesignModel();

        #endregion

        #region Constructor

        public TimerGroupDesignModel()
        {
            ID = 0;
            Title = "GroupName";
        }

        #endregion
    }
}
