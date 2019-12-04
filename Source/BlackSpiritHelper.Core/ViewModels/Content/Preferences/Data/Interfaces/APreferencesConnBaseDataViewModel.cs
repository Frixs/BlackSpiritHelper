﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class APreferencesConnBaseDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Identifier representing particular connection.
        /// </summary>
        [XmlIgnore]
        public abstract PreferencesConnectionType Identifier { get; protected set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to activate connection.
        /// </summary>
        [XmlIgnore]
        public ICommand ActivateCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public APreferencesConnBaseDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            ActivateCommand = new RelayCommand(async () => await ActivateCommandMethodAsync());
        }

        /// <summary>
        /// Activate connection method async wrapper.
        /// </summary>
        /// <returns></returns>
        private async Task ActivateCommandMethodAsync()
        {
            ActivateMethod();
            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send message to the user's connection.
        /// </summary>
        /// <param name="message"></param>
        public abstract bool SendTextMessage(string message);

        /// <summary>
        /// Activate method as the one ehich user uses.
        /// </summary>
        /// <returns></returns>
        public void ActivateMethod()
        {
            // TODO: validation stuff
            Console.WriteLine(Identifier);

            IoC.DataContent.PreferencesData.Connection.ActivateMethod(Identifier);
        }

        #endregion
    }
}