using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices
{
    public class UserProcessingService:IUserProcessingService
    {
        public User CreateUser(string fullname,string email,string password, string accessToken, string refreshToken)
        {
            var fullName=FullName.Create(fullname);
            User createdUser = User.Create(fullName.Data, email, password, accessToken, refreshToken);
            return createdUser;

        }
    }
}
