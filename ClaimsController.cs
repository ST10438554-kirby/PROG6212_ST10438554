using System.Threading.Tasks;
using ClaimSystem.Data;
using ClaimSystem.Models;
using ClaimSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimSystem.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class ClaimsController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ClaimsController(IClaimService claimService, UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _claimService = claimService;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim)
        {
            if (!ModelState.IsValid) return View(claim);

            var userId = _userManager.GetUserId(User);
            await _claimService.SubmitClaimAsync(claim, Request.Form.Files, userId);

            TempData["Success"] = "Claim submitted.";
            return RedirectToAction(nameof(MyClaims));
        }

        [HttpGet]
        public async Task<IActionResult> MyClaims()
        {
            var userId = _userManager.GetUserId(User);
            var claims = await _db.Claims
                .Include(c => c.SupportingDocuments)
                .Where(c => c.LecturerId == userId)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();
            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var claim = await _claimService.GetByIdAsync(id);
            if (claim == null) return NotFound();

            // Only owner or approver roles can view
            var userId = _userManager.GetUserId(User);
            if (claim.LecturerId != userId && !User.IsInRole("Coordinator") && !User.IsInRole("Manager"))
                return Forbid();

            return View(claim);
        }

        // Download document (secure)
        [HttpGet]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var doc = await _db.SupportingDocuments.FindAsync(id);
            if (doc == null) return NotFound();

            var claim = await _db.Claims.FindAsync(doc.ClaimId);
            var userId = _userManager.GetUserId(User);
            if (claim.LecturerId != userId && !User.IsInRole("Coordinator") && !User.IsInRole("Manager"))
                return Forbid();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doc.StoredFileName.Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(path)) return NotFound();

            var stream = System.IO.File.OpenRead(path);
            return File(stream, "application/octet-stream", doc.FileName);
        }
    }
}
