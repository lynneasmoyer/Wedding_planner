using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Contexts
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options){}
        public DbSet<User> Users {get; set;}

        public DbSet<Rsvp> Rsvps {get; set;}

        public DbSet<Wedding> Weddings {get; set;}

        
    }
}