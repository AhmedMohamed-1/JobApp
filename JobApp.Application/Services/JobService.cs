using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using JobApp.Domain.Entities;
using JobApp.Domain.Enums;

namespace JobApp.Application.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobResponseDto> CreateJobAsync(CreateJobDto dto, int userId)
    {
        var job = new Job
        {
            Title = dto.Title,
            Description = dto.Description,
            Company = dto.Company,
            Location = dto.Location,
            Status = JobStatus.Active,
            CreatedById = userId
        };

        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChangesAsync();

        return MapToDto(job);
    }

    public async Task<JobApplicationResponseDto> ApplyToJobAsync(int jobId, ApplyJobDto dto, int userId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null || job.IsDeleted || job.Status == JobStatus.Canceleld)
        {
            throw new KeyNotFoundException("Job not found or is no longer active.");
        }

        if (job.CreatedById == userId)
        {
            throw new InvalidOperationException("The creator of the job cannot apply to their own job.");
        }

        var alreadyApplied = await _jobRepository.HasUserAppliedAsync(jobId, userId);
        if (alreadyApplied)
        {
            throw new InvalidOperationException("You have already applied to this job.");
        }

        var application = new JobApplication
        {
            JobId = jobId,
            ApplicantId = userId,
            CvLink = dto.CvLink,
            Status = JobStatus.Applied,
            AppliedAt = DateTime.UtcNow
        };

        await _jobRepository.AddApplicationAsync(application);
        await _jobRepository.SaveChangesAsync();

        return new JobApplicationResponseDto
        {
            Id = application.Id,
            JobId = application.JobId,
            ApplicantId = application.ApplicantId,
            CvLink = application.CvLink,
            Status = application.Status,
            AppliedAt = application.AppliedAt
        };
    }

    public async Task<JobResponseDto> CancelJobAsync(int jobId, int userId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null || job.IsDeleted)
        {
            throw new KeyNotFoundException("Job not found.");
        }

        if (job.CreatedById != userId)
        {
            throw new UnauthorizedAccessException("Only the creator of this job can cancel it.");
        }

        job.Status = JobStatus.Canceleld;
        _jobRepository.Update(job);
        await _jobRepository.SaveChangesAsync();

        return MapToDto(job);
    }

    public async Task<JobResponseDto> ReactivateJobAsync(int jobId, int userId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null || job.IsDeleted)
        {
            throw new KeyNotFoundException("Job not found.");
        }

        if (job.CreatedById != userId)
        {
            throw new UnauthorizedAccessException("Only the creator of this job can reactivate it.");
        }

        if (job.Status != JobStatus.Canceleld)
        {
            throw new InvalidOperationException("Only canceled jobs can be reactivated.");
        }

        job.Status = JobStatus.Active;
        _jobRepository.Update(job);
        await _jobRepository.SaveChangesAsync();

        return MapToDto(job);
    }

    public async Task DeleteJobAsync(int jobId, int userId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null || job.IsDeleted)
        {
            throw new KeyNotFoundException("Job not found.");
        }

        if (job.CreatedById != userId)
        {
            throw new UnauthorizedAccessException("Only the creator of this job can delete it.");
        }

        // Soft Delete
        job.IsDeleted = true;
        job.DeletedAt = DateTime.UtcNow;

        _jobRepository.Update(job);
        await _jobRepository.SaveChangesAsync();
    }
    // Get all active jobs
    public async Task<IEnumerable<JobResponseDto>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllActiveAsync();
        return jobs.Select(MapToDto);
    }

    public async Task<JobResponseDto?> GetJobByIdAsync(int jobId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        return job == null ? null : MapToDto(job);
    }

    private static JobResponseDto MapToDto(Job job)
    {
        return new JobResponseDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Company = job.Company,
            Location = job.Location,
            Status = job.Status,
            CreatedById = job.CreatedById,
            CreatedAt = job.CreatedAt
        };
    }
}
