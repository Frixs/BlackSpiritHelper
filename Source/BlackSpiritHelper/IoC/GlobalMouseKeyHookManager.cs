using BlackSpiritHelper.Core;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;

namespace BlackSpiritHelper
{
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
        private Keys mOverlayInteractionKey = Keys.LMenu;

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
        public void SetOverlayInteractionKey(OverlayInteractionKey key)
        {
            switch (key)
            {
                case OverlayInteractionKey.LeftAlt:
                    mOverlayInteractionKey = Keys.LMenu;
                    break;

                case OverlayInteractionKey.RightAlt:
                    mOverlayInteractionKey = Keys.RMenu;
                    break;

                case OverlayInteractionKey.LeftControl:
                    mOverlayInteractionKey = Keys.LControlKey;
                    break;

                case OverlayInteractionKey.RightControl:
                    mOverlayInteractionKey = Keys.RControlKey;
                    break;

                case OverlayInteractionKey.LeftShift:
                    mOverlayInteractionKey = Keys.LShiftKey;
                    break;

                case OverlayInteractionKey.RightShift:
                    mOverlayInteractionKey = Keys.RShiftKey;
                    break;

                default:
                    IoC.Logger.Log("No key specified!", LogLevel.Warning);
                    break;
            }
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

            //It is recommened to dispose it
            mGlobalHook.Dispose();
        }

        #endregion

        #region Hook Methods

        /// <summary>
        /// Key Up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MGlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            ActionOverlaySetTransparent(sender, e);
        }

        /// <summary>
        /// Key Down event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MGlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            ActionOverlayUnsetTransparent(sender, e);
        }

        #endregion

        #region Event Solution Methods

        /// <summary>
        /// Overlay set transparecy.
        /// </summary>
        private void ActionOverlaySetTransparent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != mOverlayInteractionKey)
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
            if (e.KeyCode != mOverlayInteractionKey)
                return;

            if (IsOverlayInteractionKeyPressed(e))
                return;

            if (OverlayWindow.Window == null || !OverlayWindow.IsActionTransparent)
                return;

            OverlayWindow.IsActionTransparent = false;
            OverlayWindow.Window.UnsetWindowExTransparent();
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

            if (mOverlayInteractionKey == Keys.LMenu)
                result = e.Alt;
            else if (mOverlayInteractionKey == Keys.RMenu)
                result = e.Alt;
            else if (mOverlayInteractionKey == Keys.LControlKey)
                result = e.Control;
            else if (mOverlayInteractionKey == Keys.RControlKey)
                result = e.Control;
            else if (mOverlayInteractionKey == Keys.LShiftKey)
                result = e.Shift;
            else if (mOverlayInteractionKey == Keys.RShiftKey)
                result = e.Shift;
            
            return result;
        }

        #endregion
    }
}
