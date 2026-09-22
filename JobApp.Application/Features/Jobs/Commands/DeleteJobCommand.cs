using JobApp.Application.Interfaces;
using MediatR;

namespace JobApp.Application.Features.Jobs.Commands;

public record DeleteJobCommand(int JobId, int UserId) : IRequest;

public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand>
{
    private readonly IJobService _jobService;

    public DeleteJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public Task Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        => _jobService.DeleteJobAsync(request.JobId, request.UserId);
}
