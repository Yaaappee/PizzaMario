using System.Collections.ObjectModel;

namespace PizzaMario.Models
{
    public class Category : BaseModel
    {
        public Category()
        {
            MenuItems = new ObservableCollection<MenuItem>();
        }

        public string Name { get; set; }

        public virtual ObservableCollection<MenuItem> MenuItems { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}