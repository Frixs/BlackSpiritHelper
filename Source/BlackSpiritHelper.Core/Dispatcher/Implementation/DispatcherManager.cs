using System;
using System.Windows;
using System.Windows.Threading;

namespace BlackSpiritHelper.Core
{
    public class DispatcherManager : IDispatcherFactory
    {
        #region Private Members

        /// <summary>
        /// Dispatcher controlled by UI thread.
        /// </summary>
        private readonly IDispatcher mUI = new DisprUiThread();

        #endregion

        #region Public Properties

        /// <summary>
        /// Dispatcher controlled by UI thread.
        /// </summary>
        public IDispatcher UI => mUI;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DispatcherManager()
        {
        }

        #endregion

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the System.Windows.Threading.Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="method">The delegate to a method that takes parameters specified in args, which is pushed onto the System.Windows.Threading.Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be null.</param>
        /// <returns>An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
        public DispatcherOperation BeginInvoke(Dispatcher dispatcher, Delegate method, params object[] args)
        {
            return dispatcher.BeginInvoke(method, args);
        }
    }
}
