using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderDemo.CleanVertical.Web.Domain.GuestUserAggregate;

namespace OrderDemo.CleanVertical.Web.Infrastructure.Data.Config;

public class GuestUserConfiguration : IEntityTypeConfiguration<GuestUser>
{
  public void Configure(EntityTypeBuilder<GuestUser> builder)
  {
    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenGuidIdValueGenerator<AppDbContext, GuestUser, GuestUserId>>()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.Email)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();
  }
}
