using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;

namespace PizzaSanMorino.Models
{
    public class Client : BaseModel
    {
        private ICollection<Adress> _adresses;

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = "DATETIME('now')")]
        public DateTime RegistrationDate { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Adress> Adresses
        {
            get => _adresses ?? (_adresses = new HashSet<Adress>());
            set => _adresses = value;
        }
    }
}
