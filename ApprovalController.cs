using System.Threading.Tasks;
using ClaimSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    [Authorize(Roles = "Coordinator,Manager")]
    public class ApprovalController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly UserManager<IdentityUser> _userManager;

        public ApprovalController(IClaimService claimService, UserManager<IdentityUser> userManager)
        {
            _claimService = claimService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Pending()
        {
            var pending = await _claimService.GetPendingClaimsAsync();
            return View(pending);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string comment)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Coordinator";
            await _claimService.ApproveOrRejectAsync(id, true, user.Id, user.UserName, role, comment);
            TempData["Success"] = "Claim approved.";
            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string comment)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Coordinator";
            await _claimService.ApproveOrRejectAsync(id, false, user.Id, user.UserName, role, comment);
            TempData["Success"] = "Claim rejected.";
            return RedirectToAction(nameof(Pending));
        }
    }
}
