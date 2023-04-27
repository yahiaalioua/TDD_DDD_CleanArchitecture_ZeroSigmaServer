using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;

namespace ZeroSigma.Application.Authentication.Queries.NewSession
{
    public record NewSessionQuery
    (       
        string accessToken,
        string refreshToken
        ): IRequest<Result<string>>;
}
