using Microsoft.EntityFrameworkCore;
using PeoplesDb.Models;

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
            modelBuilder.Entity<Person>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");
        }
    }
}
