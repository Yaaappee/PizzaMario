using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PizzaMario.Models;
using PizzaMario.Views;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    public class MenuItemEditViewModel : ViewModelBase
    {
        private readonly int _currentMenuItemId;
        private ObservableCollection<Category> _categories;

        private Category _currentCategory;

        private string _name;

        private double _price;

        public MenuItemEditViewModel(MenuItem menuItem)
        {
            LoadCategories();

            if (menuItem != null)
            {
                _currentMenuItemId = menuItem.Id;
                Name = menuItem.Name;
                CurrentCategory = Categories.First(x => x.Id == menuItem.CategoryId);
                Price = menuItem.Price;
            }

            ClickSaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            ClickCancelChangesCommand = new DelegateCommand(CancelChanges);
            ClickAddCategoryCommand = new DelegateCommand(AddCategory);
            ClickUpdateCategoryCommand = new DelegateCommand(UpdateCategory, CanUpdateDeleteCategory);
            ClickDeleteCategoryCommand = new DelegateCommand(DeleteCategory, CanUpdateDeleteCategory);
        }

        public ICommand ClickSaveChangesCommand { get; }

        public ICommand ClickCancelChangesCommand { get; }

        public ICommand ClickUpdateCategoryCommand { get; }

        public ICommand ClickDeleteCategoryCommand { get; }

        public ICommand ClickAddCategoryCommand { get; }

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
                (ClickAddCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickUpdateCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
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

        private bool CanUpdateDeleteCategory()
        {
            return CurrentCategory != null;
        }

        private void LoadCategories()
        {
            using (var context = new PizzaDbContext())
            {
                var categories = new ObservableCollection<Category>(context.Categories);
                if (Categories == null)
                {
                    Categories = categories;
                }
                else
                {
                    Categories.Clear();
                    Categories.AddRange(categories);
                }
            }

            (ClickAddCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClickUpdateCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClickDeleteCategoryCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private void DeleteCategory()
        {
            var result = MessageBox.Show("Are you sure?", "Delete category", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new PizzaDbContext())
                {
                    var cat = context.Categories.First(x => x.Id == CurrentCategory.Id);
                    context.Categories.Remove(cat);
                    context.SaveChanges();
                }

                LoadCategories();
            }
        }

        private void UpdateCategory()
        {
            var viewModel = new CategoryEditViewModel(CurrentCategory);
            var view = new CategoryEdit();
            viewModel.CloseWindowEvent += (s, e) => view.Close();
            view.DataContext = viewModel;
            view.ShowDialog();
            LoadCategories();
        }

        private void AddCategory()
        {
            var viewModel = new CategoryEditViewModel(null);
            var view = new CategoryEdit();
            viewModel.CloseWindowEvent += (s, e) => view.Close();
            view.DataContext = viewModel;
            view.ShowDialog();
            LoadCategories();
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