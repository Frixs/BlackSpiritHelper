using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Handles anything to do with Tasks.
    /// </summary>
    public class TaskManager : ITaskManager
    {
        #region Task Methods

        public async Task Run(Func<Task> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                await Task.Run(function);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(function, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task<TResult> Run<TResult>(Func<Task<TResult>> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(function);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(function, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task<TResult> Run<TResult>(Func<TResult> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(function);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task Run(Func<Task> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(function, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task Run(Action action, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(action, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        public Task Run(Action action, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // Try and run the task.
                return Task.Run(action);
            }
            catch (Exception ex)
            {
                // Log error.
                LogError(ex, origin, filePath, lineNumber);

                // Throw it as normal.
                throw;
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Logs the given error to the log factory.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        private void LogError(Exception ex, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            IoC.Logger.Log($"An unexpected error occured running a IoC.Task.Run. {ex.Message}", LogLevel.Debug, origin, filePath, lineNumber);
        }

        #endregion
    }
}
