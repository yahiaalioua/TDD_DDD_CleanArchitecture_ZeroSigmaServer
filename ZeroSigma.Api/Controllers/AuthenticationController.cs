using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroSigma.Application.Common.Authentication;

namespace ZeroSigma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public AuthenticationController(IRefreshTokenProvider refreshTokenProvider)
        {
            _refreshTokenProvider = refreshTokenProvider;
        }

        [HttpGet]
        public IActionResult test()
        {
            var response = _refreshTokenProvider.GenerateRefreshToken(Guid.NewGuid(), "test@mail.com");
            return Ok(response);
        }
    }
}
