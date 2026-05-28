using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nimble.Modulith.Users.Data;

public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
  public UsersDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();
    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NimbleModulith;Trusted_Connection=True;MultipleActiveResultSets=true");

    return new UsersDbContext(optionsBuilder.Options);
  }
}
