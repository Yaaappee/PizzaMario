using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;

namespace PizzaSanMorino.Models
{
    public class Order : BaseModel
    {
        public Order()
        {
            OrderItems = new ObservableCollection<OrderItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = "DATETIME('now')")]
        public DateTime Date { get; set; }

        public double TotalPrice { get; set; }

        public virtual ObservableCollection<OrderItem> OrderItems { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int? AdressId { get; set; }

        public virtual Adress Adress { get; set; }
    }
}