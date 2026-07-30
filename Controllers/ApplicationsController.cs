using System.Security.Claims;
using JobTrackr.Data;
using JobTrackr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackr.Controllers;

[Authorize]
public class ApplicationsController(IApplicationRepository repository, IWebHostEnvironment environment) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index(string? search, ApplicationStatus? status, string? company)
    {
        var all = await repository.GetForUserAsync(UserId, search, status, company);
        ViewBag.Companies = all.Select(x => x.CompanyName).Distinct().Order(); ViewBag.Search = search; ViewBag.Status = status; ViewBag.Company = company;
        return View(all);
    }

    public IActionResult Create() => View(new JobApplication());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobApplication application)
    { if (!ModelState.IsValid) return View(application); application.UserId = UserId; await repository.AddAsync(application); await repository.SaveAsync(); TempData["Success"] = "Application added to your tracker."; return RedirectToAction(nameof(Index)); }

    public async Task<IActionResult> Edit(int id) { var app = await repository.GetOwnedAsync(id, UserId); return app is null ? NotFound() : View(app); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, JobApplication form)
    { var app = await repository.GetOwnedAsync(id, UserId); if (app is null) return NotFound(); if (!ModelState.IsValid) return View(form); app.CompanyName = form.CompanyName; app.JobTitle = form.JobTitle; app.Location = form.Location; app.Salary = form.Salary; app.ApplicationDate = form.ApplicationDate; app.ClosingDate = form.ClosingDate; app.JobDescription = form.JobDescription; app.RecruiterName = form.RecruiterName; app.RecruiterEmail = form.RecruiterEmail; app.Status = form.Status; await repository.SaveAsync(); TempData["Success"] = "Application updated."; return RedirectToAction(nameof(Details), new { id }); }

    public async Task<IActionResult> Details(int id) { var app = await repository.GetOwnedAsync(id, UserId); return app is null ? NotFound() : View(app); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id) { var app = await repository.GetOwnedAsync(id, UserId); if (app is null) return NotFound(); repository.Remove(app); await repository.SaveAsync(); TempData["Success"] = "Application removed."; return RedirectToAction(nameof(Index)); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddInterview(int id, Interview interview)
    { var app = await repository.GetOwnedAsync(id, UserId); if (app is null) return NotFound(); if (!ModelState.IsValid || interview.ScheduledAt == default) { TempData["Error"] = "Please add an interview date and type."; return RedirectToAction(nameof(Details), new { id }); } interview.JobApplicationId = id; app.Interviews.Add(interview); if (app.Status == ApplicationStatus.Applied) app.Status = ApplicationStatus.Interviewing; await repository.SaveAsync(); return RedirectToAction(nameof(Details), new { id }); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(int id, IFormFile file, string documentType)
    { var app = await repository.GetOwnedAsync(id, UserId); if (app is null) return NotFound(); if (file is null || file.Length == 0 || file.Length > 5_000_000) { TempData["Error"] = "Select a file up to 5 MB."; return RedirectToAction(nameof(Details), new { id }); } var permitted = new[] { ".pdf", ".doc", ".docx" }; var extension = Path.GetExtension(file.FileName).ToLowerInvariant(); if (!permitted.Contains(extension)) { TempData["Error"] = "Only PDF, DOC, and DOCX files are allowed."; return RedirectToAction(nameof(Details), new { id }); } var folder = Path.Combine(environment.WebRootPath, "uploads", UserId); Directory.CreateDirectory(folder); var stored = $"{Guid.NewGuid()}{extension}"; await using var stream = System.IO.File.Create(Path.Combine(folder, stored)); await file.CopyToAsync(stream); app.Documents.Add(new ApplicationDocument { OriginalFileName = Path.GetFileName(file.FileName), StoredFileName = stored, DocumentType = documentType }); await repository.SaveAsync(); return RedirectToAction(nameof(Details), new { id }); }

    public async Task<IActionResult> Analytics()
    { return View(new DashboardViewModel { Applications = await repository.GetForUserAsync(UserId, null, null, null) }); }
}
