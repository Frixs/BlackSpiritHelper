﻿using System;
using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wraps the data related to specific APM session
    /// </summary>
    public class ApmCalculatorSessionDataViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Counter list to measure the APM
        /// </summary>
        private readonly Queue<DateTime> mApmCounter = new Queue<DateTime>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Total actions of the session
        /// </summary>
        public uint TotalActions { get; set; }

        /// <summary>
        /// Saves the highest APM
        /// </summary>
        public uint HighestApm { get; set; }

        /// <summary>
        /// Current APM
        /// </summary>
        public uint CurrentApm { get; set; }

        /// <summary>
        /// Average APM
        /// </summary>
        public uint AverageApm => (uint)Math.Round(TotalActions / ElapsedTime.TotalMinutes);

        /// <summary>
        /// Total elapsed time of the session
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// Indication if the session tracks keyboard
        /// </summary>
        public bool TrackKeyboard { get; set; }

        /// <summary>
        /// Indication if the session tracks mouse clicks
        /// </summary>
        public bool TrackMouseClick { get; set; }

        /// <summary>
        /// Indication if the session tracks mouse double clicks
        /// </summary>
        public bool TrackMouseDoubleClick { get; set; }

        /// <summary>
        /// Indicates if the session tracks mouse wheel
        /// </summary>
        public bool TrackMouseWheel { get; set; }

        /// <summary>
        /// Indicates if the session tracks mouse drag
        /// </summary>
        public bool TrackMouseDrag { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Count new action
        /// </summary>
        public void CountAction()
        {
            // Count the action
            ++TotalActions;
            mApmCounter.Enqueue(DateTime.UtcNow);

            // Set the current APM
            CurrentApm = (uint)mApmCounter.Count;

            // Check for the highest APM...
            if (CurrentApm > HighestApm)
                HighestApm = CurrentApm;
        }

        /// <summary>
        /// Process the session data (in real-time)
        /// </summary>
        public void ProcessAtRealTime()
        {
            int c = mApmCounter.Count;

            // Remove old records over 1 minute...
            for (int i = 0; i < c; ++i)
            {
                if (mApmCounter.Peek() + TimeSpan.FromSeconds(60) < DateTime.UtcNow)
                    _ = mApmCounter.Dequeue();
            }

            // Let update the average APM calculation
            OnPropertyChanged(nameof(AverageApm));
        }

        #endregion
    }
}