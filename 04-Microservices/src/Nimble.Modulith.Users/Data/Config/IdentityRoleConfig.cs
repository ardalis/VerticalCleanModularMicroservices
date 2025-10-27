using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nimble.Modulith.Users.Data.Config;

public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
{
  public void Configure(EntityTypeBuilder<IdentityRole> builder)
  {
    // Seed Admin and Customer roles
    builder.HasData(
      new IdentityRole
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Admin",
        NormalizedName = "ADMIN",
        ConcurrencyStamp = Guid.NewGuid().ToString()
      },
      new IdentityRole
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Customer",
        NormalizedName = "CUSTOMER",
        ConcurrencyStamp = Guid.NewGuid().ToString()
      }
    );
  }
}
