using JobApp.Application.DTOs;

namespace JobApp.Application.Interfaces;

public interface IJobService
{
    Task<JobResponseDto> CreateJobAsync(CreateJobDto dto, int userId);
    Task<JobResponseDto> CancelJobAsync(int jobId, int userId);
    Task<JobResponseDto> ReactivateJobAsync(int jobId, int userId);
    Task DeleteJobAsync(int jobId, int userId);
    Task<JobApplicationResponseDto> ApplyToJobAsync(int jobId, ApplyJobDto dto, int userId);
    Task<IEnumerable<JobResponseDto>> GetAllJobsAsync();
    Task<JobResponseDto?> GetJobByIdAsync(int jobId);
}
