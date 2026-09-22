using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record ReactivateJobCommand(int JobId, int UserId) : IRequest<JobResponseDto>;

public class ReactivateJobCommandHandler : IRequestHandler<ReactivateJobCommand, JobResponseDto>
{
    private readonly IJobService _jobService;

    public ReactivateJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobResponseDto> Handle(ReactivateJobCommand request, CancellationToken cancellationToken)
        => _jobService.ReactivateJobAsync(request.JobId, request.UserId);
}
