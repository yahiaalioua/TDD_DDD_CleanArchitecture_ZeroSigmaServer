using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Infrastructure.Persistance.Repositories.Users;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class ApplicationDbContextTests : TestBase
    {
        private readonly FullName _fullname;
        private readonly UserEmail _email;
        private readonly UserPassword _password;
        private readonly User _user;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextTests()
        {
            _fullname = FullName.Create("Mike").Data;
            _email = UserEmail.Create("test@mail.com").Data;
            _password = UserPassword.Create("TestPass3459$%.").Data;
            _user = User.Create(_fullname,_email,_password);
            _context=DbContext;
        }
        [Fact]
        public async Task DatabaseIsAvailableAndCanBeConnectedTo()
        {
            //assert
            Assert.True(await _context.Database.CanConnectAsync());
        }
    }
}
