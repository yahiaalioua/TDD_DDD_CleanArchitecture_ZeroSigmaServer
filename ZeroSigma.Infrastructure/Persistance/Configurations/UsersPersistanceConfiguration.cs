using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Infrastructure.Persistance.Configurations
{
    public class UsersPersistanceConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasConversion
                (userId => userId.Value,
                value => UserID.Create(value)
                );
            builder.Property(u => u.FullName).HasConversion
                (fullName => fullName.Value,
                value => FullName.Create(value).Data
                );
            builder.Property(u => u.Email).HasConversion
                (email => email.Value,
                value => UserEmail.Create(value).Data
                );
            builder.Property(u => u.Password).HasConversion
                (password => password.Value,
                value => UserPassword.Create(value).Data
                );
            builder.Property(u => u.FullName).HasMaxLength(50);
            builder.Property(u=>u.Email).HasMaxLength(100);
            builder.Property(u=>u.Password).HasMaxLength(70);
        }
    }
}
