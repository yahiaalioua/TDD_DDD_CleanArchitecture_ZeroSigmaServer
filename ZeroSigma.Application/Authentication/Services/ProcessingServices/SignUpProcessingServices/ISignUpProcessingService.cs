using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.SignUpProcessingServices
{
    public interface ISignUpProcessingService
    {
        User CreateUser(FullName fullname, UserEmail email, UserPassword password);
        User? ProcessSignUpRequest(FullName fullname, UserEmail email, UserPassword password);
    }
}
