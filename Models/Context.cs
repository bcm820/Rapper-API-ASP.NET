using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIntro.Models {

  public class Context : DbContext {

    // Db Tables
    public DbSet<User> Users { get; set; }
    // public DbSet<Artists> Artists { get; set; }
    // public DbSet<Songs> Songs { get; set; }

    // Context calls parent class' constructor
    // passing in the "options" parameter
    public Context(DbContextOptions<Context> options) : base(options) { }
  }

}