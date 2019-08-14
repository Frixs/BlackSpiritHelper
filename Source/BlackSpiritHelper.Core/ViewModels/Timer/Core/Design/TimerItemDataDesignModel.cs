using System;

namespace BlackSpiritHelper.Core
{
    public class TimerItemDataDesignModel : TimerItemDataViewModel
    {
        #region New Instance Getter

        public static TimerItemDataDesignModel Instance => new TimerItemDataDesignModel();

        #endregion

        #region Constructor

        public TimerItemDataDesignModel()
        {
            GroupID = 0;
            Title = "New Timer";
            IconTitleShortcut = "NT";
            IconBackgroundHEX = "FA2C9B";
            TimeDuration = new TimeSpan(0, 1, 0);
            CountdownDuration = TimeSpan.FromSeconds(3);
            State = TimerState.Ready;
            IsLoopActive = false;
        }

        #endregion
    }
}
