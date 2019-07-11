using System;

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
            TimerGroupViewModel g = AddGroup("Default");
            g.AddTimer(new TimerItemViewModel
            {
                GroupID = 0,
                Title = "Cool Timer",
                IconTitleShortcut = "CT",
                IconBackgroundHEX = "1f61cc",
                TimeTotal = new TimeSpan(0, 1, 30),
                CountdownDuration = TimeSpan.FromSeconds(3),
                State = TimerState.Ready,
                IsRunning = false,
                IsLoopActive = true,
                ShowInOverlay = true,
                IsWarningTime = false,
            });
            AddGroup("Test Group");
            CanCreateNewGroup = true;
        }

        #endregion
    }
}
