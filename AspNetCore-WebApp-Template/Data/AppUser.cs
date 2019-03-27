using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabasePerTenantPOC.Utilities;

namespace DatabasePerTenantPOC.Data
{
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Represents the tenant's shard key in the database
        /// </summary>
        public int TenantId { get; set; }

    }
}
