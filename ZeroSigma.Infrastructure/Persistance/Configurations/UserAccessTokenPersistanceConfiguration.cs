using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;

namespace ZeroSigma.Infrastructure.Persistance.Configurations
{
    public class UserAccessTokenPersistanceConfiguration : IEntityTypeConfiguration<UserAccessToken>
    {
        public void Configure(EntityTypeBuilder<UserAccessToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion
                (
                userAccessTokenId => userAccessTokenId.Value,
                value => AccessTokenID.Create(value)
                );
            builder.Property(x => x.IsExpired).HasMaxLength(2);
            builder.Property(x => x.AccessToken).HasMaxLength(600);
            builder.HasOne<UserAccess>().WithOne().HasForeignKey<UserAccess>(x => x.AccessTokenID);
        }
    }
}
