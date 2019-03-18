using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Data.CatalogDB
{
    public class Tenants
    {
        public byte[] TenantId { get; set; }
        public string TenantName { get; set; }
        public string ServicePlan { get; set; }
        public string RecoveryState { get; set; }
        public System.DateTime LastUpdated { get; set; }
    }
}
