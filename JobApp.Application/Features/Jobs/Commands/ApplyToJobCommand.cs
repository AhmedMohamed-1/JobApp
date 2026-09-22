using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record ApplyToJobCommand(int JobId, ApplyJobDto Dto, int UserId) : IRequest<JobApplicationResponseDto>;

public class ApplyToJobCommandHandler : IRequestHandler<ApplyToJobCommand, JobApplicationResponseDto>
{
    private readonly IJobService _jobService;

    public ApplyToJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobApplicationResponseDto> Handle(ApplyToJobCommand request, CancellationToken cancellationToken)
        => _jobService.ApplyToJobAsync(request.JobId, request.Dto, request.UserId);
}
