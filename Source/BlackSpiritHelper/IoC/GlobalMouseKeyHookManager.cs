using BlackSpiritHelper.Core;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;

namespace BlackSpiritHelper
{
    public class GlobalMouseKeyHookManager : IMouseKeyHook
    {
        #region Private Members

        private IKeyboardMouseEvents mGlobalHook;

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionOverlaySetTransparent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.LMenu)
                return;
            
            // If the key was not pressed, it is not the action we are looking for to disable.
            if (!e.Alt)
                return;
            
            if (OverlayWindow.Window == null || OverlayWindow.IsActionTransparent)
                return;

            OverlayWindow.IsActionTransparent = true;
            OverlayWindow.Window.SetWindowExTransparent();
        }

        /// <summary>
        /// Overlay unset transparecy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionOverlayUnsetTransparent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.LMenu)
                return;
            
            if (e.Alt)
                return;
            
            if (OverlayWindow.Window == null || !OverlayWindow.IsActionTransparent)
                return;

            OverlayWindow.IsActionTransparent = false;
            OverlayWindow.Window.UnsetWindowExTransparent();
        }

        #endregion
    }
}
