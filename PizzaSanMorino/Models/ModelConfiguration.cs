using System.Data.Entity;

namespace PizzaSanMorino.Models
{
    public static class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureClientEntity(modelBuilder);
            ConfigureAdressEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);
            ConfigureMenuItemEntity(modelBuilder);
            ConfigureOrderEntity(modelBuilder);
            ConfigureOrderItemEntity(modelBuilder);
        }

        private static void ConfigureClientEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>();
        }

        private static void ConfigureAdressEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adress>()
                .HasRequired(t => t.Client)
                .WithMany(c => c.Adresses)
                .WillCascadeOnDelete(true);
        }

        private static void ConfigureCategoryEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
        }

        private static void ConfigureMenuItemEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .HasRequired(t => t.Category)
                .WithMany(c => c.MenuItems)
                .WillCascadeOnDelete(true);
        }

        private static void ConfigureOrderEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasRequired(t => t.Client)
                .WithMany(c => c.Orders)
                .WillCascadeOnDelete(true);
        }

        private static void ConfigureOrderItemEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasRequired(t => t.MenuItem)
                .WithMany(c => c.OrderItems)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderItem>()
                .HasRequired(t => t.Order)
                .WithMany(c => c.OrderItems)
                .WillCascadeOnDelete(true);
        }
    }
}
