using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System.Windows.Input;
using PizzaSanMorino.Models;
using PizzaSanMorino.Views;
using PizzaSanMorino.Utils;
using Prism.Commands;

namespace PizzaSanMorino.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly IDialogService DialogService;
        public ICommand ClickUpdateClientCommand
        {
            get;
        }
        public ICommand ClickDeleteClientCommand
        {
            get;
        }
        public ICommand ClickAddClientCommand
        {
            get;
        }

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "PizzaSanMorino"; }
        }

        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumberToSearch))
                    return clients;
                else
                    return new ObservableCollection<Client>(clients.Where(x => x.PhoneNumber.Contains(PhoneNumberToSearch)));
            }
            set
            {
                clients = value;
                NotifyPropertyChanged();
            }
        }

        private Client currentClient;

        public Client CurrentClient
        {
            get { return currentClient; }
            set
            {
                if (value == currentClient) return;
                currentClient = value;
                NotifyPropertyChanged("CurrentClient");
                (ClickUpdateClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string phoneNumberToSearch;

        public string PhoneNumberToSearch
        {
            get { return phoneNumberToSearch; }
            set
            {
                phoneNumberToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Clients");
            }
        }

        public MainViewModel()
        {
            // DialogService is used to handle dialogs
            this.DialogService = new MvvmDialogs.DialogService();
            LoadClients();
            ClickUpdateClientCommand = new DelegateCommand(UpdateClient, CanUpdateClient);
            ClickDeleteClientCommand = new DelegateCommand(DeleteClient, CanUpdateClient);
            ClickAddClientCommand = new DelegateCommand(AddClient);
        }

        public void LoadClients()
        {
            using (var context = new PizzaDbContext())
            {
                if (Clients == null)
                    clients = new ObservableCollection<Client>();
                else
                    clients.Clear();
                clients.AddRange(context.Clients);
                NotifyPropertyChanged("Clients");
            }
        }

        public void UpdateClient()
        {
            var app = new ClientEdit();
            var viewModel = new ClientEditViewModel(CurrentClient);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadClients();
        }

        public bool CanUpdateClient()
        {
            return CurrentClient != null;
        }

        public void AddClient()
        {
            var app = new ClientEdit();
            var viewModel = new ClientEditViewModel(null);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadClients();
        }

        public void DeleteClient()
        {
            using (var context = new PizzaDbContext())
            {
                var client = context.Clients.FirstOrDefault(x => x.Id == CurrentClient.Id);
                if (client == null) return;
                context.Clients.Remove(client);

                context.SaveChanges();
            }

            LoadClients();
        }

        public RelayCommand<object> SampleCmdWithArgument { get { return new RelayCommand<object>(OnSampleCmdWithArgument); } }

        public ICommand SaveAsCmd { get { return new RelayCommand(OnSaveAsTest, AlwaysFalse); } }
        public ICommand SaveCmd { get { return new RelayCommand(OnSaveTest, AlwaysFalse); } }
        public ICommand NewCmd { get { return new RelayCommand(OnNewTest, AlwaysFalse); } }
        public ICommand OpenCmd { get { return new RelayCommand(OnOpenTest, AlwaysFalse); } }
        public ICommand ShowAboutDialogCmd { get { return new RelayCommand(OnShowAboutDialog, AlwaysTrue); } }
        public ICommand ExitCmd { get { return new RelayCommand(OnExitApp, AlwaysTrue); } }

        private bool AlwaysTrue() { return true; }
        private bool AlwaysFalse() { return false; }

        private void OnSampleCmdWithArgument(object obj)
        {
            // TODO
        }

        private void OnSaveAsTest()
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "Save As",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false,
                OverwritePrompt = true
            };

            bool? success = DialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                // Do something
                Log.Info("Saving file: " + settings.FileName);
            }
        }
        private void OnSaveTest()
        {
            // TODO
        }
        private void OnNewTest()
        {
            // TODO
        }
        private void OnOpenTest()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Open",
                Filter = "Sample (.xml)|*.xml",
                CheckFileExists = false
            };

            bool? success = DialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                // Do something
                Log.Info("Opening file: " + settings.FileName);
            }
        }
        private void OnShowAboutDialog()
        {
            Log.Info("Opening About dialog");
            AboutViewModel dialog = new AboutViewModel();
            var result = DialogService.ShowDialog<About>(this, dialog);
        }
        private void OnExitApp()
        {
            System.Windows.Application.Current.MainWindow.Close();
        }
    }
}
