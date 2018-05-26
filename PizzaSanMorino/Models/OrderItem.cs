﻿namespace PizzaSanMorino.Models
{
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public int Quantity { get; set; }
    }
}