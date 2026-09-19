using System.Security.Claims;
using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobApp.Controllers;

/// <summary>
/// Manages job postings, job applications, and application cancellation flows.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    /// <summary>
    /// Retrieves all active job postings.
    /// </summary>
    /// <returns>A list of active jobs available for application.</returns>
    /// <response code="200">Returns list of active job postings.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobResponseDto>>> GetAll()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    /// <summary>
    /// Retrieves details of a specific job posting by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the job.</param>
    /// <returns>Job details if found.</returns>
    /// <response code="200">Returns the job details.</response>
    /// <response code="404">Job not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponseDto>> GetById(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);
        if (job == null) return NotFound(new { message = "Job not found." });
        return Ok(job);
    }

    /// <summary>
    /// Creates a new job posting.
    /// </summary>
    /// <param name="dto">The job details payload.</param>
    /// <returns>The created job posting.</returns>
    /// <response code="201">Job created successfully.</response>
    /// <response code="401">Unauthorized. Requires a valid JWT token.</response>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<JobResponseDto>> Create([FromBody] CreateJobDto dto)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        var createdJob = await _jobService.CreateJobAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = createdJob.Id }, createdJob);
    }

    /// <summary>
    /// Submits a job application for the specified job posting.
    /// </summary>
    /// <param name="id">The ID of the job to apply to.</param>
    /// <param name="dto">Application details containing CV link.</param>
    /// <returns>The created job application.</returns>
    /// <response code="200">Application submitted successfully.</response>
    /// <response code="400">User already applied or creator applying to own job.</response>
    /// <response code="401">Unauthorized. Requires candidate JWT token.</response>
    /// <response code="404">Job not found or inactive.</response>
    [Authorize]
    [HttpPost("{id:int}/apply")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationResponseDto>> Apply(int id, [FromBody] ApplyJobDto dto)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        try
        {
            var application = await _jobService.ApplyToJobAsync(id, dto, userId);
            return Ok(application);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancels the authenticated user's job application for the specified job.
    /// </summary>
    /// <remarks>
    /// Candidates who applied for a job and changed their mind can use this endpoint to cancel their application.
    /// Only the candidate who originally submitted the application is authorized to cancel it.
    /// An application that is already cancelled cannot be cancelled again.
    /// </remarks>
    /// <param name="id">The ID of the job whose application should be cancelled.</param>
    /// <returns>The updated job application with cancelled status.</returns>
    /// <response code="200">Application cancelled successfully.</response>
    /// <response code="400">Application is already cancelled.</response>
    /// <response code="401">Unauthorized. Requires a valid JWT token.</response>
    /// <response code="403">Forbidden. Only the applicant who applied can cancel this application.</response>
    /// <response code="404">No application found for this job by the current user.</response>
    [Authorize]
    [HttpPut("{id:int}/cancel-application")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationResponseDto>> CancelApplication(int id)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        try
        {
            var cancelledApp = await _jobService.CancelApplicationAsync(id, userId);
            return Ok(cancelledApp);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancels a job posting (Employer / Job Creator action).
    /// </summary>
    /// <param name="id">The ID of the job to cancel.</param>
    /// <returns>The updated job with Canceleld status.</returns>
    /// <response code="200">Job cancelled successfully.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden. Only the creator of the job can cancel it.</response>
    /// <response code="404">Job not found.</response>
    [Authorize]
    [HttpPut("{id:int}/cancel")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponseDto>> Cancel(int id)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        try
        {
            var cancelledJob = await _jobService.CancelJobAsync(id, userId);
            return Ok(cancelledJob);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Reactivates a cancelled job posting (Employer / Job Creator action).
    /// </summary>
    /// <param name="id">The ID of the job to reactivate.</param>
    /// <returns>The updated job with Active status.</returns>
    /// <response code="200">Job reactivated successfully.</response>
    /// <response code="400">Only cancelled jobs can be reactivated.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden. Only the creator of the job can reactivate it.</response>
    /// <response code="404">Job not found.</response>
    [Authorize]
    [HttpPut("{id:int}/reactivate")]
    [ProducesResponseType(typeof(JobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponseDto>> Reactivate(int id)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        try
        {
            var reactivatedJob = await _jobService.ReactivateJobAsync(id, userId);
            return Ok(reactivatedJob);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Soft deletes a job posting (Employer / Job Creator action).
    /// </summary>
    /// <param name="id">The ID of the job to delete.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Job deleted successfully.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden. Only the creator of the job can delete it.</response>
    /// <response code="404">Job not found.</response>
    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0) return Unauthorized();

        try
        {
            await _jobService.DeleteJobAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
}
