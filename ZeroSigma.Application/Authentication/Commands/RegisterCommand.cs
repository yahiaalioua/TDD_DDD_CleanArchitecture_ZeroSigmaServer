using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;

namespace ZeroSigma.Application.Authentication.Commands
{
    public record RegisterCommand
    (
        string FullName,
        string Email,
        string Password
        ) : IRequest<Result<SignUpResponse>>;
}
