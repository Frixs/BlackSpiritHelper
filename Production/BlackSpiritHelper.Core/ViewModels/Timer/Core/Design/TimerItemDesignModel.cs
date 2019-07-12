using System;

namespace BlackSpiritHelper.Core
{
    public class TimerItemDesignModel : TimerItemViewModel
    {
        #region Singleton

        public static TimerItemDesignModel Instance => new TimerItemDesignModel();

        #endregion

        #region Constructor

        public TimerItemDesignModel()
        {
            GroupID = 0;
            Title = "New Timer";
            IconTitleShortcut = "NT";
            IconBackgroundHEX = "FA2C9B";
            TimeTotal = new TimeSpan(0, 1, 0);
            CountdownDurationTotal = TimeSpan.FromSeconds(3);
            State = TimerState.Ready;
            IsLoopActive = false;
        }

        #endregion
    }
}
