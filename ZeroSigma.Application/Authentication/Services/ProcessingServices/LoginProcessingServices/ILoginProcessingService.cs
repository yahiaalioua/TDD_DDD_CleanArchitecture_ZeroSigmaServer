﻿using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices
{
    public interface ILoginProcessingService
    {
        JwtSecurityToken DecodeJwt(string token);
        DateTime GetTokenExpiryDate(string token);
        DateTime GetTokenIssueDate(string token);
        Task PersistIdentity(User user, string accessToken,string refreshToken);
        Task ProcessAuthentication(User user,string accessToken,string refreshToken);
    }
}