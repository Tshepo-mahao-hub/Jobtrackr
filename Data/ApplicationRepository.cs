using JobTrackr.Models;
using Microsoft.EntityFrameworkCore;
namespace JobTrackr.Data;
public class ApplicationRepository(ApplicationDbContext context) : IApplicationRepository
{
    public async Task<List<JobApplication>> GetForUserAsync(string userId, string? search, ApplicationStatus? status, string? company)
    { var q = context.JobApplications.Include(x => x.Interviews).Where(x => x.UserId == userId).AsQueryable(); if (!string.IsNullOrWhiteSpace(search)) q = q.Where(x => x.CompanyName.Contains(search) || x.JobTitle.Contains(search)); if (status.HasValue) q = q.Where(x => x.Status == status); if (!string.IsNullOrWhiteSpace(company)) q = q.Where(x => x.CompanyName == company); return await q.OrderByDescending(x => x.ApplicationDate).ToListAsync(); }
    public Task<JobApplication?> GetOwnedAsync(int id, string userId) => context.JobApplications.Include(x => x.Interviews).Include(x => x.Documents).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    public async Task AddAsync(JobApplication application) => await context.JobApplications.AddAsync(application);
    public Task SaveAsync() => context.SaveChangesAsync(); public void Remove(JobApplication application) => context.JobApplications.Remove(application);
}
