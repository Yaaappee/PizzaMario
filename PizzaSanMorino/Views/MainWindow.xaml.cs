using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using log4net;
using PizzaSanMorino.Models;


namespace PizzaSanMorino.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainView_Closing;
            using (var context = new PizzaDbContext())
            {
                var client = context.Clients.First();
                var outputString = client.FirstName + " " + client.SecondName;
                var outputString2 = string.Empty;
                foreach (Adress adress in client.Adresses)
                {
                    outputString += Environment.NewLine + adress.City + " " + adress.Street;
                }

                MessageBox.Show(outputString + Environment.NewLine + outputString2);
            }
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
                if (((MainViewModel)(this.DataContext)).Data.IsModified)
                if (!((MainViewModel)(this.DataContext)).PromptSaveBeforeExit())
                {
                    e.Cancel = true;
                    return;
                }
            */
            Log.Info("Closing App");
        }
    }
}
