using Microsoft.EntityFrameworkCore;
namespace Alsabbah_Rawan_CsharpExam.Models
{

    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Actvity> Activities { get; set; }
        public DbSet<Partspent> Partspents { get; set; }
    }
}