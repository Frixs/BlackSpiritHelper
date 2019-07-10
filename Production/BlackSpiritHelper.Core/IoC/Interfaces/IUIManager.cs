﻿using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The UI manager that handles any UI interaction in the application.
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// DIsplays a single message box to the user.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        Task ShowMessage(MessageBoxDialogViewModel viewModel);
    }
}
