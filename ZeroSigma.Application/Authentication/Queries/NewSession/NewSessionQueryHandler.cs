using MediatR;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Authentication.Queries.NewSession
{
    public class NewSessionQueryHandler : IRequestHandler<NewSessionQuery, Result<NewSessionResponse>>
    {
        private readonly INewSessionProcessingService _sessionProcessingService;
        public NewSessionQueryHandler(
            INewSessionProcessingService sessionProcessingService
            )
        {
            _sessionProcessingService = sessionProcessingService;
        }

        public async Task<Result<NewSessionResponse>> Handle(NewSessionQuery request, CancellationToken cancellationToken)
        {
            NewSessionRequest newSessionrequest = new(request.accessToken, request.refreshToken);
            return _sessionProcessingService.ProcessNewSession(newSessionrequest);
        }
    }
}
