using Anything.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anything.Api.Infrastructure.Database
{
    public class EntitiesDbContext : DbContext
    {
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<Entity> Entities { get; set; }

        public EntitiesDbContext()
        {
        }

        public EntitiesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Entities;Trusted_Connection=True;Application Name=Anything.Api;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EntityType
            var entityTypeBuilder = modelBuilder.Entity<EntityType>();
            entityTypeBuilder.
                HasKey(x => x.Id);
            entityTypeBuilder
                .Property(x => x.Name)
                .HasMaxLength(256)
                .IsRequired();

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique()
                .HasName("UIX_Name");

            // Entity
            var entityBuilder = modelBuilder.Entity<Entity>();
            entityBuilder.
                HasKey(x => x.Id);

            entityBuilder
                .HasIndex(x => x.EntityTypeId)
                .HasName("IX_EntityTypeId");

            entityBuilder.
                Property(x => x.Data)
                .IsRequired();
        }
    }
}
