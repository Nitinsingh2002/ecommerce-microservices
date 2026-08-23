

using Authentication.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("Refresh token");

        builder.HasKey(x => x.Id);

        // creating a index on the Token column in Refresh Token table for efficient searching of tokens
        builder.HasIndex(x => x.Token).IsUnique();

        builder.Property(X => X.Token).HasMaxLength(1000).IsRequired();

        builder.Property(X => X.ExpiredAt).IsRequired();

        builder.Property(X => X.IsRevoked).HasDefaultValue(false);

        builder.HasOne(X => X.User).WithMany(X => X.RefreshTokens).HasForeignKey(X => X.UserId).OnDelete(DeleteBehavior.Cascade);

    }
}