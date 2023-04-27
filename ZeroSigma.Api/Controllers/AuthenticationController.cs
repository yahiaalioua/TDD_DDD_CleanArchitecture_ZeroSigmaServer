using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.Authentication.Queries;
using ZeroSigma.Application.Authentication.Queries.NewSession;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Infrastructure.Authentication.AccessToken;

namespace ZeroSigma.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AccessTokenOptions _accessTokenOptions;
        private readonly INewSessionProcessingService _sessionProcessingService;

        public AuthenticationController(
            IMediator mediator, IOptions<AccessTokenOptions> options,
            INewSessionProcessingService sessionProcessingService
            )
        {
            _mediator = mediator;
            _accessTokenOptions = options.Value;
            _sessionProcessingService = sessionProcessingService;
        }

        [HttpPost("users")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            
            var command = new RegisterCommand(request.FullName, request.Email, request.Password);
            var response = await _mediator.Send(command);
            return this.FromResult(response);
        }
        [HttpPost("session")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = new LoginQuery(request.Email, request.Password);
            var response = await _mediator.Send(command);
            
            if(response.Data!= null)
            {
                SetTokenCookie(response.Data.AccessToken);
            }
            
            return this.FromResult(response);
        }
        [HttpPost("session/new")]
        public async Task<IActionResult> RefreshToken(NewSessionRequest request)
        {
            var query = new NewSessionQuery(request.accessToken, request.refreshToken);
            var response= await _mediator.Send(query);
            return this.FromResult(response);
        }
        private void SetTokenCookie(string token)
        {
            // append cookie with access token to the http response
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenOptions.AccessTokenExpirationMinutes)
            };
            Response.Cookies.Append("accessToken", token, cookieOptions);
        }
    }
}
