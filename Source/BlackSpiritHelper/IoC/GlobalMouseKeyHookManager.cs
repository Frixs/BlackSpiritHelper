using BlackSpiritHelper.Core;
using Gma.System.MouseKeyHook;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackSpiritHelper
{
    /// <summary>
    /// The application global user interaction hook manager
    /// <para>
    ///     The manager tracks the minimum needed interaction for the proper application use.
    ///     The application does not track user communication and there is no intention to do it.
    /// </para>
    /// </summary>
    public class GlobalMouseKeyHookManager : IMouseKeyHook
    {
        #region Private Members

        /// <summary>
        /// Hook eventer
        /// </summary>
        private IKeyboardMouseEvents mGlobalHook;

        /// <summary>
        /// Interaction key for overlay
        /// </summary>
        private Keys mOverlayInteractionHotkey = Keys.LMenu;

        /// <summary>
        /// Interaction key for APM Calculator start/stop
        /// </summary>
        private Keys mApmCalculatorStartStopHotkey = Keys.None;

        /// <summary>
        /// Indicates if any keybind is currently being captured
        /// </summary>
        private bool mCapturingHotkey = false;

        /// <summary>
        /// Indicates captured key (string representation) while capturing keybind
        /// </summary>
        private string mCapturedHotkey = null;

        /// <summary>
        /// APM Calculator session data reference that are currently subsbribed
        /// </summary>
        private ApmCalculatorSessionDataViewModel mApmCalculatorSessionData;

        #endregion

        #region Constructor/Dispose

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GlobalMouseKeyHookManager()
        {
            Subscribe();
        }

        /// <summary>
        /// Dispose instance.
        /// </summary>
        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion

        #region Interface Methods

        /// <inheritdoc/>
        public void SetOverlayInteractionHotkey(OverlayInteractionKey key)
        {
            switch (key)
            {
                case OverlayInteractionKey.LeftAlt:
                    mOverlayInteractionHotkey = Keys.LMenu;
                    break;

                case OverlayInteractionKey.RightAlt:
                    mOverlayInteractionHotkey = Keys.RMenu;
                    break;

                case OverlayInteractionKey.LeftControl:
                    mOverlayInteractionHotkey = Keys.LControlKey;
                    break;

                case OverlayInteractionKey.RightControl:
                    mOverlayInteractionHotkey = Keys.RControlKey;
                    break;

                case OverlayInteractionKey.LeftShift:
                    mOverlayInteractionHotkey = Keys.LShiftKey;
                    break;

                case OverlayInteractionKey.RightShift:
                    mOverlayInteractionHotkey = Keys.RShiftKey;
                    break;

                default:
                    IoC.Logger.Log("No key specified!", LogLevel.Warning);
                    break;
            }
        }

        /// <inheritdoc/>
        public void SetApmCalculatorControlHotkey(string key)
        {
            if (key == null)
                mApmCalculatorStartStopHotkey = Keys.None;

            if (Enum.TryParse(key, out Keys theKey))
                mApmCalculatorStartStopHotkey = theKey;
            else
                mApmCalculatorStartStopHotkey = Keys.None;
        }

        /// <inheritdoc/>
        public async Task<string> CaptureHotkeyAsync()
        {
            // Do no allow more than 1 capturing at the time
            if (mCapturingHotkey)
                return null;
            mCapturingHotkey = true;

            string result = null;

            // Hook the event for the capture
            mGlobalHook.KeyUp += CaptureHotkey_KeyUp;

            // Give a user some time to set a keybind (or continue once the keybind is set)
            for (int i = 0; i < 200; ++i) // 100 = 10sec
            {
                if (mCapturedHotkey != null)
                    break;
                await Task.Delay(100);
            }

            if (!mCapturedHotkey.Equals(Keys.Escape.ToString())) // Escape is unset button
                result = mCapturedHotkey;

            // Dispose the capture process
            mGlobalHook.KeyUp -= CaptureHotkey_KeyUp;
            mCapturedHotkey = null;
            mCapturingHotkey = false;

            return result;
        }

        /// <inheritdoc/>
        public void SubscribeApmCalculatorEvents(ApmCalculatorSessionDataViewModel sessionData)
        {
            mApmCalculatorSessionData = sessionData;

            if (mApmCalculatorSessionData.TrackKeyboard)
                mGlobalHook.KeyUp += ApmCalculator_KeyUp;
            if (mApmCalculatorSessionData.TrackMouseClick)
                mGlobalHook.MouseClick += ApmCalculator_MouseClick;
            if (mApmCalculatorSessionData.TrackMouseDoubleClick)
                mGlobalHook.MouseDoubleClick += ApmCalculator_MouseDoubleClick;
            if (mApmCalculatorSessionData.TrackMouseWheel)
                mGlobalHook.MouseWheel += ApmCalculator_MouseWheel;
            if (mApmCalculatorSessionData.TrackMouseDrag)
                mGlobalHook.MouseDragFinished += ApmCalculator_MouseDragFinished;
        }

        /// <inheritdoc/>
        public void UnsubscribeApmCalculatorEvents()
        {
            mGlobalHook.KeyUp -= ApmCalculator_KeyUp;
            mGlobalHook.MouseClick -= ApmCalculator_MouseClick;
            mGlobalHook.MouseDoubleClick -= ApmCalculator_MouseDoubleClick;
            mGlobalHook.MouseWheel -= ApmCalculator_MouseWheel;
            mGlobalHook.MouseDragFinished -= ApmCalculator_MouseDragFinished;

            mApmCalculatorSessionData = null;
        }

        #endregion

        #region Subscribe/Unsubscribe Methods

        /// <summary>
        /// Subscribe.
        /// </summary>
        private void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead.
            mGlobalHook = Hook.GlobalEvents();

            mGlobalHook.KeyDown += MGlobalHook_KeyDown;
            mGlobalHook.KeyUp += MGlobalHook_KeyUp;
        }

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        private void Unsubscribe()
        {
            mGlobalHook.KeyDown -= MGlobalHook_KeyDown;
            mGlobalHook.KeyUp -= MGlobalHook_KeyUp;

            mGlobalHook.KeyUp -= CaptureHotkey_KeyUp;

            // Make sure to unsubscribe special events
            UnsubscribeApmCalculatorEvents();

            //It is recommened to dispose it
            mGlobalHook.Dispose();
        }

        #endregion

        #region Hook Methods

        /// <summary>
        /// Key Up event handler
        /// </summary>
        private void MGlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine("KEYUP");
            ActionOverlaySetTransparent(sender, e);
            ActionApmCalculatorControl(sender, e);
        }

        /// <summary>
        /// Key Down event handler
        /// </summary>
        private void MGlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine("KEYDOWN");
            ActionOverlayUnsetTransparent(sender, e);
        }

        /// <summary>
        /// Keybind capture event
        /// </summary>
        private void CaptureHotkey_KeyUp(object sender, KeyEventArgs e)
        {
            mCapturedHotkey = e.KeyCode.ToString();
        }

        /// <summary>
        /// Key Up event handler
        /// </summary>
        private void ApmCalculator_KeyUp(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine("KEYUP");
            ActionApmCalculatorCount();
        }

        /// <summary>
        /// Mouse Click event handler
        /// </summary>
        private void ApmCalculator_MouseClick(object sender, MouseEventArgs e)
        {
            //System.Console.WriteLine("MOUSECLICK");
            ActionApmCalculatorCount();
        }

        /// <summary>
        /// Mouse Double Click event handler
        /// </summary>
        private void ApmCalculator_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //System.Console.WriteLine("MOUSEDOUBLECLICK");
            ActionApmCalculatorCount();
        }

        /// <summary>
        /// Mouse Wheel event handler
        /// </summary>
        private void ApmCalculator_MouseWheel(object sender, MouseEventArgs e)
        {
            //System.Console.WriteLine("MOUSEWHEEL");
            ActionApmCalculatorCount();
        }

        /// <summary>
        /// Mouse Drag Finished event handler
        /// </summary>
        private void ApmCalculator_MouseDragFinished(object sender, MouseEventArgs e)
        {
            //System.Console.WriteLine("MOUSEDRAGFINISHED");
            ActionApmCalculatorCount();
        }

        #endregion

        #region Event Solution Methods

        /// <summary>
        /// Overlay set transparecy.
        /// </summary>
        private void ActionOverlaySetTransparent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != mOverlayInteractionHotkey)
                return;

            // If the key was not pressed, it is not the action we are looking for to disable.
            if (!IsOverlayInteractionKeyPressed(e))
                return;

            if (OverlayWindow.Window == null || OverlayWindow.IsActionTransparent)
                return;

            OverlayWindow.IsActionTransparent = true;
            OverlayWindow.Window.SetWindowExTransparent();
        }

        /// <summary>
        /// Overlay unset transparecy.
        /// </summary>
        private void ActionOverlayUnsetTransparent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != mOverlayInteractionHotkey)
                return;

            if (IsOverlayInteractionKeyPressed(e))
                return;

            if (OverlayWindow.Window == null || !OverlayWindow.IsActionTransparent)
                return;

            OverlayWindow.IsActionTransparent = false;
            OverlayWindow.Window.UnsetWindowExTransparent();
        }

        /// <summary>
        /// APM Calculator start/stop event handler
        /// </summary>
        private void ActionApmCalculatorControl(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.None || e.KeyCode != mApmCalculatorStartStopHotkey)
                return;

            if (IoC.DataContent.ApmCalculatorData.IsRunning)
            {
                IoC.DataContent.ApmCalculatorData.StopCommand.Execute(null);
            }
            else 
            {
                if (IoC.DataContent.ApmCalculatorData.CurrentSession.TotalActions > 0)
                    IoC.DataContent.ApmCalculatorData.RestartCommand.Execute(null);
                else
                    IoC.DataContent.ApmCalculatorData.PlayCommand.Execute(null);
            }
        }

        /// <summary>
        /// Count APM process
        /// </summary>
        private void ActionApmCalculatorCount()
        {
            if (mApmCalculatorSessionData == null)
                return;

            mApmCalculatorSessionData.CountAction();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Select the appropriate key based on user selected overlay interaction key and evaluate press
        /// </summary>
        /// <param name="e">event args from event method</param>
        private bool IsOverlayInteractionKeyPressed(KeyEventArgs e)
        {
            bool result = false;

            if (mOverlayInteractionHotkey == Keys.LMenu)
                result = e.Alt;
            else if (mOverlayInteractionHotkey == Keys.RMenu)
                result = e.Alt;
            else if (mOverlayInteractionHotkey == Keys.LControlKey)
                result = e.Control;
            else if (mOverlayInteractionHotkey == Keys.RControlKey)
                result = e.Control;
            else if (mOverlayInteractionHotkey == Keys.LShiftKey)
                result = e.Shift;
            else if (mOverlayInteractionHotkey == Keys.RShiftKey)
                result = e.Shift;

            return result;
        }

        #endregion
    }
}
