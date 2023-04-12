﻿using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Infrastructure.Authentication;
using ZeroSigma.Infrastructure.Authentication.AccessToken;

namespace ZeroSigma.Infrastructure.Tests.Authentication
{
    public class JwtTests
    {
        [Fact]
        public void ShouldGenerateJwt()
        {
            //arrange
            string secretKey = "Secretkey763232gjhdudssafdsgsdeyr843r8hwueifbuoahf834hioahefiojewqug72";
            var JwtGenerator = new JwtGenrator();
            //act
            string JwtToken = JwtGenerator.GenerateJwt(secretKey,"issuer","Audience",10);
            //assert
            Assert.True(JwtToken.Length > 50);
        }

        [Fact]
        public void ShouldGenerateAccessToken()
        {
            //arrange
            var id=Guid.NewGuid();
            var name = "yahia";
            var email = "test@email.com";
            var accessTokenOptions = new AccessTokenOptions() {
                AccessTokenExpirationMinutes = 10,
                AccessTokenSecretKey = "Supersjdfsudhbew8732821jjhwqhygdygewq66a8",
                Audience="testAudience",
                Issuer="testIssuer"
            };
            var IOptions = Options.Create(accessTokenOptions);
            IJwtGenerator jwtGenerator=new JwtGenrator();

            
            var AccessTokenProvider = new AccessTokenProvider(jwtGenerator,IOptions);


            //act
            string accesToken=AccessTokenProvider.GenerateAccessToken(id, name, email);
            var jwt=new JwtSecurityTokenHandler().ReadJwtToken(accesToken);

            //assert
            Assert.True(accesToken.Length > 50);
            Assert.Equal(name, jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value);
            Assert.Equal(email, jwt.Claims.First(x => x.Type == ClaimTypes.Email).Value);
            Assert.Equal(id.ToString(), jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);


        }
    }
}
