using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PizzaMario.Models;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    public class MenuItemEditViewModel : ViewModelBase
    {
        public event EventHandler CloseWindowEvent;

        public ICommand ClickSaveChangesCommand { get; }

        public ICommand ClickCancelChangesCommand { get; }

        private int currentMenuItemId;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Category currentCategory;

        public Category CurrentCategory
        {
            get { return currentCategory; }
            set
            {
                currentCategory = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Category> categories;

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                NotifyPropertyChanged();
            }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public MenuItemEditViewModel(MenuItem menuItem)
        {
            using (var context = new PizzaDbContext())
            {
                Categories = new ObservableCollection<Category>(context.Categories);
            }

            if (menuItem != null)
            {
                currentMenuItemId = menuItem.Id;
                Name = menuItem.Name;
                CurrentCategory = Categories.First(x => x.Id == menuItem.CategoryId);
                Price = menuItem.Price;
            }

            ClickSaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            ClickCancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        public void SaveChanges()
        {
            using (var context = new PizzaDbContext())
            {
                if (currentMenuItemId == 0)
                {
                    context.MenuItems.Add(new MenuItem()
                    {
                        Name = Name,
                        Price = Price,
                        CategoryId = CurrentCategory.Id
                    });
                    context.SaveChanges();
                }
                else
                {
                    var menuItem = context.MenuItems.First(x => x.Id == currentMenuItemId);
                    menuItem.Name = Name;
                    menuItem.Price = Price;
                    menuItem.CategoryId = CurrentCategory.Id;
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
            return !string.IsNullOrWhiteSpace(Name)
                   && Price > 0
                   && CurrentCategory != null;
        }
    }
}
