using JobTrackr.Models;
namespace JobTrackr.Data;
public interface IApplicationRepository
{
    Task<List<JobApplication>> GetForUserAsync(string userId, string? search, ApplicationStatus? status, string? company);
    Task<JobApplication?> GetOwnedAsync(int id, string userId);
    Task AddAsync(JobApplication application); Task SaveAsync(); void Remove(JobApplication application);
}
