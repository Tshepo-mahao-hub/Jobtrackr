using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackr.Controllers;

/// <summary>Reserved administration endpoint demonstrating role-based authorization.</summary>
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index() => Content("JobTrackr administration");
}
