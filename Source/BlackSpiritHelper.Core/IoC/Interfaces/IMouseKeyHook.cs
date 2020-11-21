using System;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Tracks global mouse & keyboard events.
    /// </summary>
    public interface IMouseKeyHook : IDisposable
    {
        /// <summary>
        /// Set overlay interaction key according to ovelray key representation
        /// </summary>
        void SetOverlayInteractionKey(OverlayInteractionKey key);

        /// <summary>
        /// Subscribe APM Calculator events
        /// </summary>
        /// <param name="sessionData">The session data to start</param>
        void SubscribeApmCalculatorEvents(ApmCalculatorSessionDataViewModel sessionData);

        /// <summary>
        /// Unsubscribe APM Calculator events
        /// </summary>
        void UnsubscribeApmCalculatorEvents();
    }
}
