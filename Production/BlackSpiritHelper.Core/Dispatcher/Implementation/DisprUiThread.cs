using System;
using System.Windows;
using System.Windows.Threading;

namespace BlackSpiritHelper.Core
{
    public class DisprUiThread : IDispatcher
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DisprUiThread()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the System.Windows.Threading.Dispatcher was created on.
        /// ---
        /// If dispatcher does not exist, delegate method will not be done.
        /// </summary>
        /// <param name="method">The delegate to a method that takes parameters specified in args, which is pushed onto the System.Windows.Threading.Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be null.</param>
        /// <returns>An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
        public DispatcherOperation BeginInvokeOrDie(Delegate method, params object[] args)
        {
            // TODO: Possibly, we can remove the whole dispatcher structure, because we can call...
            // return Application.Current?.Dispatcher.BeginInvoke(method, args);
            // ... as a null check.
            // Make sure about functionality before.
            if (Application.Current == null)
                return null;

            return Application.Current.Dispatcher.BeginInvoke(method, args);
        }

        #endregion
    }
}
