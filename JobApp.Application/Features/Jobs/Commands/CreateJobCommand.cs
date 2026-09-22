using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record CreateJobCommand(CreateJobDto Dto, int UserId) : IRequest<JobResponseDto>;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, JobResponseDto>
{
    private readonly IJobService _jobService;

    public CreateJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobResponseDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        => _jobService.CreateJobAsync(request.Dto, request.UserId);
}
