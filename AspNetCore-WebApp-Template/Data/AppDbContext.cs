using DatabasePerTenantPOC.Data;
using DatabasePerTenantPOC.Data.CustomerDB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatabasePerTenantPOC
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }        
    }
}