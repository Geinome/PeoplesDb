using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeoplesDb.Shared;

namespace PeoplesDb.Api.People.Models
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Person> personEntity = modelBuilder.Entity<Person>();
            personEntity.HasKey(p => p.Id);
            personEntity.Property(b => b.FirstName)
                .IsRequired();
            personEntity.Property(b => b.LastName)
                .IsRequired();
        }
    }
}
