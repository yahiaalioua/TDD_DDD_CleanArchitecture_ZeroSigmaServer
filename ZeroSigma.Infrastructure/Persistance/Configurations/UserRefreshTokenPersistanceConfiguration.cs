using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance.Configurations
{
    public class UserRefreshTokenPersistanceConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion
                (
                refreshTokenId => refreshTokenId.Value,
                value => RefreshTokenID.Create(value)
                );
            builder.Property(x => x.IsExpired).HasMaxLength(2);
            builder.Property(x => x.RefreshToken).HasMaxLength(1000);
            builder.HasOne<UserAccess>().WithOne().HasForeignKey<UserAccess>(x => x.RefreshTokenID);

        }
    }
}
