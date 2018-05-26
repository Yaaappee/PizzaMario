using System.Collections.Generic;

namespace PizzaSanMorino.Models
{
    public class MenuItem : BaseModel
    {
        public MenuItem()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public string Name { get; set; }

        public double Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}