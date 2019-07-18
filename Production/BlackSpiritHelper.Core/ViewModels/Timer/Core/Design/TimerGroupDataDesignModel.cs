namespace BlackSpiritHelper.Core
{
    public class TimerGroupDataDesignModel : TimerGroupDataViewModel
    {
        #region New Instance Getter

        public static TimerGroupDataDesignModel Instance => new TimerGroupDataDesignModel();

        #endregion

        #region Constructor

        public TimerGroupDataDesignModel()
        {
            ID = 0;
            Title = "GroupName";
        }

        #endregion
    }
}
