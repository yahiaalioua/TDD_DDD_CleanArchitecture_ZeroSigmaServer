using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Infrastructure.Persistance.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _ctx;

        public UserRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User?> GetByIdAsync(UserID id)
        {
            return await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User?> GetByEmailAsync(UserEmail email)
        {
            return await _ctx.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task AddUserAsync(User user)
        {
            await _ctx.Users.AddAsync(user);
            _ctx.SaveChanges();
        }
    }
}
