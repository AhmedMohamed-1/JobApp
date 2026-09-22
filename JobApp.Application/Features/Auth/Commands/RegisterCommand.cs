using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterDto Dto) : IRequest<AuthResponseDto>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        => _authService.RegisterAsync(request.Dto);
}
