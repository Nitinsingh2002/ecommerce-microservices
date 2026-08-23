

using Authentication.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(X => X.LastName).HasMaxLength(50).IsRequired();
        builder.Property(X => X.CreatedAt).IsRequired();
        builder.Property(X => X.IsDeleted).HasDefaultValue(false);
    }
}