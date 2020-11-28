using System;
using System.Threading.Tasks;

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
        void SetOverlayInteractionHotkey(OverlayInteractionKey key);

        /// <summary>
        /// Set APM Calculator keybind
        /// </summary>
        void SetApmCalculatorControlHotkey(string key);

        /// <summary>
        /// Capture key procedure for keybind
        /// </summary>
        /// <returns>String representation of the selected keybind</returns>
        Task<string> CaptureHotkeyAsync();

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
