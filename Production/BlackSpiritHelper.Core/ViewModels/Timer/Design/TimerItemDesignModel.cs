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
            TimeFormat = "00:02:30";
            CountdownDuration = TimeSpan.FromSeconds(3);
            State = TimerState.Ready;
            IsRunning = false;
            IsLoopActive = false;
            IsWarningTime = false;
        }

        #endregion
    }
}
