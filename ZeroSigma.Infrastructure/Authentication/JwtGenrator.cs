﻿using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Infrastructure.Authentication
{
    public class JwtGenrator:IJwtGenerator
    {

        public string GenerateJwt(string secretKey, string issuer, string audience, double expires, List<Claim> claims=null!)
        {
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            
            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: signinCredentials
                );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}