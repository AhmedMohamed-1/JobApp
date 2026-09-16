using JobApp.Domain.Entities;

namespace JobApp.Application.Interfaces;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(int id);
    Task<IEnumerable<Job>> GetAllAsync();
    Task<IEnumerable<Job>> GetAllActiveAsync();
    Task AddAsync(Job job);
    void Update(Job job);
    void Delete(Job job);

    Task AddApplicationAsync(JobApplication application);
    Task<JobApplication?> GetApplicationAsync(int jobId, int applicantId);
    Task<bool> HasUserAppliedAsync(int jobId, int applicantId);

    Task SaveChangesAsync();
}
