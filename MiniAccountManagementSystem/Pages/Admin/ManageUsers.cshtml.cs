using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using MiniAccountManagementSystem.Models;  // Add this to access ApplicationUser
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniAccountManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserWithRole> Users { get; set; }
        public List<string> AllRoles { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public class UserWithRole
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string CurrentRole { get; set; }
        }

        public async Task OnGetAsync()
        {
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            Users = new List<UserWithRole>();

            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRole
                {
                    Id = user.Id,
                    Email = user.Email,
                    CurrentRole = roles.FirstOrDefault() ?? "None"
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null && !string.IsNullOrWhiteSpace(SelectedRole))
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, SelectedRole);
            }

            return RedirectToPage();
        }
    }
}
