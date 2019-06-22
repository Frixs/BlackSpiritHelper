using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BlackSpiritTimerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string LogFileLocation = "Log/";
        public const string LogFileName = "Log.txt";
        public const string LogFilePath = LogFileLocation + LogFileName;
    }
}
