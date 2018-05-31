using System;
using System.Data.Entity;
using SQLite.CodeFirst;

namespace PizzaMario.Models
{
    public class PizzaDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<PizzaDbContext>
    {
        public PizzaDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Seed(PizzaDbContext context)
        {
            var categoryPizza = new Category
            {
                Id = 1,
                Name = "Пицца"
            };

            var categoryDrinks = new Category
            {
                Id = 2,
                Name = "Напитки"
            };

            var menuItemPizzaMargarita = new MenuItem
            {
                Id = 1,
                CategoryId = categoryPizza.Id,
                Name = "Маргарита",
                Price = 80.99
            };

            var menuItemPizzaBbq = new MenuItem
            {
                Id = 2,
                CategoryId = categoryPizza.Id,
                Name = "BBQ",
                Price = 102.99
            };

            var menuItemFiveCheese = new MenuItem
            {
                Id = 3,
                CategoryId = categoryPizza.Id,
                Name = "Четыре сыра",
                Price = 153.99
            };

            var menuItemCola = new MenuItem
            {
                Id = 4,
                CategoryId = categoryDrinks.Id,
                Name = "Coca Cola",
                Price = 14.99
            };

            var menuItemPepsi = new MenuItem
            {
                Id = 5,
                CategoryId = categoryDrinks.Id,
                Name = "Pepsi",
                Price = 13.99
            };

            var menuItemJuice = new MenuItem
            {
                Id = 6,
                CategoryId = categoryDrinks.Id,
                Name = "Сок",
                Price = 10.99
            };

            var menuItemAmerican = new MenuItem
            {
                Id = 7,
                CategoryId = categoryPizza.Id,
                Name = "Американка",
                Price = 111.99
            };

            var menuItemTuna = new MenuItem
            {
                Id = 8,
                CategoryId = categoryPizza.Id,
                Name = "Пицца с тунцом",
                Price = 181.99
            };

            var clientIhor = new Client
            {
                Id = 1,
                FirstName = "Игорь",
                SecondName = "Соколов",
                BirthDate = new DateTime(1992, 11, 1),
                PhoneNumber = "+380991234567"
            };
            
            var clientEvgeniy = new Client
            {
                Id = 2,
                FirstName = "Евгений",
                SecondName = "Федченко",
                BirthDate = new DateTime(1991, 12, 31),
                PhoneNumber = "+380997654321"
            };

            var order1 = new Order
            {
                Id = 1,
                ClientId = clientIhor.Id,
                TotalPrice = menuItemFiveCheese.Price + menuItemCola.Price * 2,
                Date = DateTime.Now
            };

            var order1Item1 = new OrderItem
            {
                Id = 1,
                MenuItemId = menuItemFiveCheese.Id,
                OrderId = order1.Id,
                Quantity = 1
            };

            var order1Item2 = new OrderItem
            {
                Id = 2,
                MenuItemId = menuItemCola.Id,
                OrderId = order1.Id,
                Quantity = 2
            };

            var order2 = new Order
            {
                Id = 2,
                ClientId = clientEvgeniy.Id,
                TotalPrice = menuItemTuna.Price + menuItemPizzaBbq.Price + menuItemPepsi.Price + menuItemJuice.Price,
                Date = DateTime.Now
            };

            var order2Item1 = new OrderItem
            {
                Id = 3,
                MenuItemId = menuItemTuna.Id,
                OrderId = order2.Id,
                Quantity = 1
            };

            var order2Item2 = new OrderItem
            {
                Id = 4,
                MenuItemId = menuItemPizzaBbq.Id,
                OrderId = order2.Id,
                Quantity = 1
            };

            var order2Item3 = new OrderItem
            {
                Id = 5,
                MenuItemId = menuItemPepsi.Id,
                OrderId = order2.Id,
                Quantity = 1
            };

            var order2Item4 = new OrderItem
            {
                Id = 6,
                MenuItemId = menuItemJuice.Id,
                OrderId = order2.Id,
                Quantity = 1
            };

            context.Clients.AddRange(new[]
            {
                clientIhor,
                clientEvgeniy
            });

            context.Categories.AddRange(new[]
            {
                categoryPizza,
                categoryDrinks
            });

            context.MenuItems.AddRange(new[]
            {
                menuItemPizzaMargarita,
                menuItemPizzaBbq,
                menuItemFiveCheese,
                menuItemCola,
                menuItemPepsi,
                menuItemJuice,
                menuItemAmerican,
                menuItemTuna
            });

            context.Orders.AddRange(new[]
            {
                order1,
                order2
            });

            context.OrderItems.AddRange(new[]
            {
                order1Item1,
                order1Item2,
                order2Item1,
                order2Item2,
                order2Item3,
                order2Item4
            });

            context.SaveChanges();
        }
    }
}