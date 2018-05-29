using System.Collections.ObjectModel;

namespace PizzaMario.Models
{
    public class MenuItem : BaseModel
    {
        public MenuItem()
        {
            OrderItems = new ObservableCollection<OrderItem>();
        }

        public string Name { get; set; }

        public double Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ObservableCollection<OrderItem> OrderItems { get; set; }
    }
}