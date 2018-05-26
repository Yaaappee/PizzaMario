using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;

namespace PizzaSanMorino.Models
{
    public class Order : BaseModel
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = "DATETIME('now')")]
        public DateTime Date { get; set; }

        public double TotalPrice { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}