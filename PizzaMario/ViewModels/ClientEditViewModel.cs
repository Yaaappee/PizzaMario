using System;
using System.Linq;
using System.Windows.Input;
using PizzaMario.Models;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    public class ClientEditViewModel : ViewModelBase
    {
        private DateTime? _birthDate;

        private readonly int _currentClientId;

        private string _firstName;

        private string _phoneNumber;

        private string _secondName;

        public ClientEditViewModel(Client client)
        {
            if (client != null)
            {
                _currentClientId = client.Id;
                FirstName = client.FirstName;
                SecondName = client.SecondName;
                PhoneNumber = client.PhoneNumber;
                BirthDate = client.BirthDate;
            }

            ClickSaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            ClickCancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        public ICommand ClickSaveChangesCommand { get; }

        public ICommand ClickCancelChangesCommand { get; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string SecondName
        {
            get => _secondName;
            set
            {
                _secondName = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CloseWindowEvent;

        public void SaveChanges()
        {
            using (var context = new PizzaDbContext())
            {
                if (_currentClientId == 0)
                {
                    context.Clients.Add(new Client
                    {
                        FirstName = FirstName,
                        SecondName = SecondName,
                        PhoneNumber = PhoneNumber,
                        BirthDate = BirthDate
                    });
                    context.SaveChanges();
                }
                else
                {
                    var client = context.Clients.First(x => x.Id == _currentClientId);
                    client.FirstName = FirstName;
                    client.SecondName = SecondName;
                    client.PhoneNumber = PhoneNumber;
                    client.BirthDate = BirthDate;
                    context.SaveChanges();
                }

                CloseWindowEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public void CancelChanges()
        {
            CloseWindowEvent?.Invoke(this, EventArgs.Empty);
        }

        public bool CanSaveChanges()
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                   && !string.IsNullOrWhiteSpace(SecondName)
                   && !string.IsNullOrWhiteSpace(PhoneNumber)
                   && BirthDate != null;
        }
    }
}