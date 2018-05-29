using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PizzaSanMorino.Models;
using PizzaSanMorino.Views;
using Prism.Commands;

namespace PizzaSanMorino.ViewModels
{
    public class ClientEditViewModel : ViewModelBase
    {
        public event EventHandler CloseWindowEvent;

        public ICommand ClickSaveChangesCommand
        {
            get;
        }

        public ICommand ClickCancelChangesCommand
        {
            get;
        }

        private int currentClientId;

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string secondName;

        public string SecondName
        {
            get { return secondName; }
            set
            {
                secondName = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private DateTime? birthDate;

        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ClientEditViewModel(Client client)
        {
            if (client != null)
            {
                currentClientId = client.Id;
                FirstName = client.FirstName;
                SecondName = client.SecondName;
                PhoneNumber = client.PhoneNumber;
                BirthDate = client.BirthDate;
            }
            ClickSaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            ClickCancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        public void SaveChanges()
        {
            using (var context = new PizzaDbContext())
            {
                if (currentClientId == 0)
                {
                    context.Clients.Add(new Client()
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
                    var client = context.Clients.First(x => x.Id == currentClientId);
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
