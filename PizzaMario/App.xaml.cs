using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using log4net;
using Microsoft.VisualBasic.Devices;
using MvvmDialogs;
using PizzaMario.ViewModels;
using PizzaMario.Views;

namespace PizzaMario
{
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static MainWindow _app;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var resourceDictionary =
                LoadComponent(new Uri("./Resources/Theme.xaml", UriKind.Relative)) as ResourceDictionary;
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(resourceDictionary);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("uk-UA");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            Log.Info("Application Startup");

            // For catching Global uncaught exception
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UnhandledExceptionOccured;

            Log.Info("Starting App");
            LogMachineDetails();
            _app = new MainWindow();
            var context = new MainViewModel();
            _app.DataContext = context;
            _app.Show();

            if (e.Args.Length == 1) //make sure an argument is passed
            {
                Log.Info("File type association: " + e.Args[0]);
                var file = new FileInfo(e.Args[0]);
                if (file.Exists) //make sure it's actually a file
                {
                    // Here, add you own code
                    // ((MainViewModel)app.DataContext).OpenFile(file.FullName);
                }
            }
        }

        private static void UnhandledExceptionOccured(object sender, UnhandledExceptionEventArgs args)
        {
            // Here change path to the log.txt file
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                       + "\\isoko\\PizzaSanMorino\\log.txt";

            // Show a message before closing application
            var dialogService = new DialogService();
            dialogService.ShowMessageBox((INotifyPropertyChanged) _app.DataContext,
                "Oops, something went wrong and the application must close. Please find a " +
                "report on the issue at: " + path + Environment.NewLine +
                "If the problem persist, please contact isoko.",
                "Unhandled Error");

            var e = (Exception) args.ExceptionObject;
            Log.Fatal("Application has crashed", e);
        }

        private void LogMachineDetails()
        {
            var computer = new ComputerInfo();

            var text = "OS: " + computer.OSPlatform + " v" + computer.OSVersion + Environment.NewLine +
                       computer.OSFullName + Environment.NewLine +
                       "RAM: " + computer.TotalPhysicalMemory + Environment.NewLine +
                       "Language: " + computer.InstalledUICulture.EnglishName;
            Log.Info(text);
        }
    }
}