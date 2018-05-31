using System;
using System.Linq;
using System.Windows.Input;
using PizzaMario.Models;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    public class CategoryEditViewModel : ViewModelBase
    {
        private readonly int _currentCategoryId;

        private string _name;

        public CategoryEditViewModel(Category category)
        {
            if (category != null)
            {
                _currentCategoryId = category.Id;
                Name = category.Name;
                Title = "Изменить категорию";
            }
            else
            {
                Title = "Добавить категорию";
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

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                NotifyPropertyChanged();
            }
        }

        public event EventHandler CloseWindowEvent;

        public void SaveChanges()
        {
            using (var context = new PizzaDbContext())
            {
                if (_currentCategoryId == 0)
                {
                    context.Categories.Add(new Category
                    {
                        Name = Name
                    });
                    context.SaveChanges();
                }
                else
                {
                    var category = context.Categories.First(x => x.Id == _currentCategoryId);
                    category.Name = Name;
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
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}