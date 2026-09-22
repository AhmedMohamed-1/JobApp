using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Auth.Commands;

public record RefreshTokenCommand(RefreshTokenRequestDto Dto) : IRequest<AuthResponseDto>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        => _authService.RefreshTokenAsync(request.Dto);
}
