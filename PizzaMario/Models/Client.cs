using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;

namespace PizzaMario.Models
{
    public class Client : BaseModel
    {
        public Client()
        {
            Orders = new ObservableCollection<Order>();
        }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = "DATETIME('now')")]
        public DateTime RegistrationDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ObservableCollection<Order> Orders { get; set; }
    }
}