using System.Data.Entity;

namespace PizzaMario.Models
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Adress> Adresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public PizzaDbContext() : base("PizzaSqlLite")
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new PizzaDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }
}
