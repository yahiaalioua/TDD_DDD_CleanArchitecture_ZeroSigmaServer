using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
