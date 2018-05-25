using System;
using System.Data.Entity;
using System.Linq;
using SQLite.CodeFirst;

namespace PizzaSanMorino.Models
{
    public class PizzaDbInitializer: SqliteDropCreateDatabaseAlways<PizzaDbContext>
    {
        public PizzaDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        { }

        protected override void Seed(PizzaDbContext context)
        {
            context.Clients.Add(new Client()
            {
                Id = 1,
                FirstName = "Ihor",
                SecondName = "Sokoliuk",
                BirthDate = new DateTime(1994, 5, 23),
                PhoneNumber = "+380991234567"
            });
            
            context.Adresses.Add(new Adress()
            {
                City = "Kiev",
                Street = "Lobody",
                BuildingNumber = "22",
                AppartmentNumber = "432",
                ClientId = 1
            });

            context.SaveChanges();
        }
    }
}
