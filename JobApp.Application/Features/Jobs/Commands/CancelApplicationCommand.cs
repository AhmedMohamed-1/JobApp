using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record CancelApplicationCommand(int JobId, int UserId) : IRequest<JobApplicationResponseDto>;

public class CancelApplicationCommandHandler : IRequestHandler<CancelApplicationCommand, JobApplicationResponseDto>
{
    private readonly IJobService _jobService;

    public CancelApplicationCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobApplicationResponseDto> Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
        => _jobService.CancelApplicationAsync(request.JobId, request.UserId);
}
