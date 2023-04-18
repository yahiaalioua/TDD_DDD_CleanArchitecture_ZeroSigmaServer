using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;

namespace ZeroSigma.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> test(RegisterRequest request)
        {
            var command = new RegisterCommand(request.FullName, request.Email, request.Password);
            var response = await _mediator.Send(command);
            return this.FromResult(response);
        }
    }
}
