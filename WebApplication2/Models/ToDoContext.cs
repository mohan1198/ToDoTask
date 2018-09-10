using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ToDoList.Models
{
    public class ToDoContext : DbContext
    {

        public DbSet<User> users { get; set; }
        public DbSet<Task> tasks { get; set; }

        public ToDoContext()
        {

        }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
            Database.Migrate();
        }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
          optionsBuilder.UseSqlServer(Startup.Configuration.GetConnectionString("DefaultConnection"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>().HasIndex(a => a.Email)
            .IsUnique();

        }


    }
}