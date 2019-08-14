using System;
using System.Windows.Threading;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Managing dispatchers.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread that the System.Windows.Threading.Dispatcher was created on.
        /// ---
        /// If dispatcher does not exist, delegate method will not be done.
        /// </summary>
        /// <param name="method">The delegate to a method that takes parameters specified in args, which is pushed onto the System.Windows.Threading.Dispatcher event queue.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be null.</param>
        /// <returns>An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
        DispatcherOperation BeginInvokeOrDie(Delegate method, params object[] args);
    }
}
