using JobApp.Application.Interfaces;
using JobApp.Domain.Entities;
using JobApp.Domain.Enums;
using JobApp.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

namespace JobApp.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);
    }

    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        return await _context.Jobs.Where(j => !j.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetAllActiveAsync()
    {
        return await _context.Jobs
            .Where(j => !j.IsDeleted && j.Status != JobStatus.Canceleld)
            .ToListAsync();
    }

    public async Task AddAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
    }

    public void Update(Job job)
    {
        _context.Jobs.Update(job);
    }

    public void Delete(Job job)
    {
        _context.Jobs.Remove(job);
    }

    public async Task AddApplicationAsync(JobApplication application)
    {
        await _context.JobApplications.AddAsync(application);
    }

    public async Task<JobApplication?> GetApplicationAsync(int jobId, int applicantId)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.JobId == jobId && ja.ApplicantId == applicantId);
    }

    public async Task<bool> HasUserAppliedAsync(int jobId, int applicantId)
    {
        return await _context.JobApplications
            .AnyAsync(ja => ja.JobId == jobId && ja.ApplicantId == applicantId);
    }

    public void UpdateApplication(JobApplication application)
    {
        _context.JobApplications.Update(application);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
