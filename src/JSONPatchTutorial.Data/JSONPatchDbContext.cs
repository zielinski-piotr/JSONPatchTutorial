using JSONPatchTutorial.Domain;
using Microsoft.EntityFrameworkCore;

namespace JSONPatchTutorial.Data
{
    public class JsonPatchDbContext : DbContext
    {
        public JsonPatchDbContext(DbContextOptions<JsonPatchDbContext> options)
            : base(options)
        {
        }

        public DbSet<House> Houses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<House>()
                .HasOne<Address>(x => x.Address);

            modelBuilder.Entity<House>()
                .OwnsMany<Room>(x => x.Rooms);

            modelBuilder.Entity<Address>()
                .HasKey(x => x.Id);
        }
    }
}