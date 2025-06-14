using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniAccountManagementSystem.Models; // Add this

namespace MiniAccountManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // use ApplicationUser
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Later you'll add DbSet<ChartOfAccount>, DbSet<Voucher>, etc.
    }
}
