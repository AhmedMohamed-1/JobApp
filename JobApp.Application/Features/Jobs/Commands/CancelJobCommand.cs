using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record CancelJobCommand(int JobId, int UserId) : IRequest<JobResponseDto>;

public class CancelJobCommandHandler : IRequestHandler<CancelJobCommand, JobResponseDto>
{
    private readonly IJobService _jobService;

    public CancelJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobResponseDto> Handle(CancelJobCommand request, CancellationToken cancellationToken)
        => _jobService.CancelJobAsync(request.JobId, request.UserId);
}
