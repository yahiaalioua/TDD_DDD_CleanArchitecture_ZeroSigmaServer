using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices
{
    public interface IUserProcessingService
    {
        User CreateUser(string fullname, string email, string password, string accessToken, string refreshToken);
    }
}
