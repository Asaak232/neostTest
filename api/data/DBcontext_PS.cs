using BaseSpace.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseSpace.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Skill>()
                .Property(s => s.Level)
                .HasDefaultValue(1)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .Property(n => n.Name)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .ToTable(s => s.HasCheckConstraint("ValidLevel", "\"Level\" >= 1 AND \"Level\" <= 10"));

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Skills)
                .WithOne(s => s.Person)
                .HasForeignKey(s => s.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .Property(n => n.Name)
                .IsRequired();

            modelBuilder.Entity<Person>()
                .Property(d => d.DisplayName)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
