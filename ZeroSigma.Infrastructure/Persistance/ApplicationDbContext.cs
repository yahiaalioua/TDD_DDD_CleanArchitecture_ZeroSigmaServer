using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserAccess> UsersAccess { get; set; } = null!;
        public DbSet<UserRefreshToken> UsersRefreshToken { get; set; } = null!;
        public DbSet<UserAccessToken> UsersAccessToken { get; set; } = null!;
        public DbSet<UserAccessBlackList> UsersAccessBlackLists { get; set; } = null!; 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
