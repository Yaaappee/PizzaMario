using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PizzaMario.Models;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    public class MenuItemEditViewModel : ViewModelBase
    {
        private ObservableCollection<Category> _categories;

        private Category _currentCategory;

        private readonly int _currentMenuItemId;

        private string _name;

        private double _price;

        public MenuItemEditViewModel(MenuItem menuItem)
        {
            using (var context = new PizzaDbContext())
            {
                Categories = new ObservableCollection<Category>(context.Categories);
            }

            if (menuItem != null)
            {
                _currentMenuItemId = menuItem.Id;
                Name = menuItem.Name;
                CurrentCategory = Categories.First(x => x.Id == menuItem.CategoryId);
                Price = menuItem.Price;
            }

            ClickSaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            ClickCancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        public ICommand ClickSaveChangesCommand { get; }

        public ICommand ClickCancelChangesCommand { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                NotifyPropertyChanged();
                (ClickSaveChangesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CloseWindowEvent;

        public void SaveChanges()
        {
            using (var context = new PizzaDbContext())
            {
                if (_currentMenuItemId == 0)
                {
                    context.MenuItems.Add(new MenuItem
                    {
                        Name = Name,
                        Price = Price,
                        CategoryId = CurrentCategory.Id
                    });
                    context.SaveChanges();
                }
                else
                {
                    var menuItem = context.MenuItems.First(x => x.Id == _currentMenuItemId);
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