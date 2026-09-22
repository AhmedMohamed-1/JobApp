using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Queries;

public record GetJobByIdQuery(int Id) : IRequest<JobResponseDto?>;

public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobResponseDto?>
{
    private readonly IJobService _jobService;

    public GetJobByIdQueryHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<JobResponseDto?> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        => _jobService.GetJobByIdAsync(request.Id);
}
