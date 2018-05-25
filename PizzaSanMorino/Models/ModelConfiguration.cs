using System.Data.Entity;

namespace PizzaSanMorino.Models
{
    public static class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureClientEntity(modelBuilder);
            ConfigureAdressEntity(modelBuilder);
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
                .WillCascadeOnDelete(false);
        }
    }
}
