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
    }
}
