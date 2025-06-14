using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MiniAccountManagementSystem.Pages.Admin
{
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(RoleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
                if (result.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            Roles = _roleManager.Roles.ToList();
            return Page();
        }
    }
}
