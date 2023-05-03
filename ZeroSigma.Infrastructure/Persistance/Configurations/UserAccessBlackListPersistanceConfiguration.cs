using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.UserAccessBlackList;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance.Configurations
{
    public class UserAccessBlackListPersistanceConfiguration : IEntityTypeConfiguration<UserAccessBlackList>
    {
        public void Configure(EntityTypeBuilder<UserAccessBlackList> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion
                (
                blacklistId => blacklistId.Value,
                value => BlackListId.Create(value)
                );
            builder.Property(x => x.RefreshTokenID).HasConversion
                (
                refreshTokenId => refreshTokenId.Value,
                value => RefreshTokenID.Create(value)
                );
            builder.Property(x => x.RevokedRefreshTokens).HasConversion
                (new ValueConverter<List<string>, string>(
                revokedRefreshToken => JsonConvert.SerializeObject(revokedRefreshToken),
                value => JsonConvert.DeserializeObject<List<string>>(value)),
                new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList())
                ).HasMaxLength(7000);
            builder.HasOne<UserRefreshToken>().WithOne().HasForeignKey<UserAccessBlackList>(x => x.RefreshTokenID);
        }
    }
}
