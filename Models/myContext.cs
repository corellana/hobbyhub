using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        // "users" table is represented by this DbSet "Users"
            public DbSet<User> Users {get;set;}
            public DbSet<Hobby> Hobbies {get;set;}
            public DbSet<Association> Associations {get;set;}
    }
}

