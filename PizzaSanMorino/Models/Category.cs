using System.Collections.Generic;

namespace PizzaSanMorino.Models
{
    public class Category : BaseModel
    {
        public Category()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public string Name { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}