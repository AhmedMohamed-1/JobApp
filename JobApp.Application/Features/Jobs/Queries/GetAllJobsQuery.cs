using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Queries;

public record GetAllJobsQuery : IRequest<IEnumerable<JobResponseDto>>;

public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<JobResponseDto>>
{
    private readonly IJobService _jobService;

    public GetAllJobsQueryHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task<IEnumerable<JobResponseDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        => _jobService.GetAllJobsAsync();
}
