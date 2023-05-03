using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccess;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance.Configurations
{
    public class UserAccessPersistanceConfiguration : IEntityTypeConfiguration<UserAccess>
    {
        public void Configure(EntityTypeBuilder<UserAccess> builder)
        {
            builder.HasKey(ua => ua.Id);
            builder.Property(ua => ua.Id).HasConversion
                (userAccessId => userAccessId.Value,
                value => UserAccessID.Create(value)
                );
            builder.Property(ua => ua.UserID).HasConversion
                (userId => userId.Value,
                value => UserID.Create(value)
                );
            builder.Property(ua => ua.AccessTokenID).HasConversion(
                accessTokenId => accessTokenId.Value,
                value => AccessTokenID.Create(value)
                );
            builder.Property(ua => ua.RefreshTokenID).HasConversion(
                refreshTokenId => refreshTokenId.Value,
                value => RefreshTokenID.Create(value)
                );
            builder.HasOne<User>().WithOne().HasForeignKey<UserAccess>(u=>u.UserID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
