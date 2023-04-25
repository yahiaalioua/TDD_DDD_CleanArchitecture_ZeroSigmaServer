using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices
{
    public class UserProcessingService:IUserProcessingService
    {
        public User CreateUser(FullName fullname,UserEmail email,UserPassword password)
        {
            User createdUser = User.Create(fullname,email,password);
            return createdUser;

        }
    }
}
