using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Models
{
    public class TenantModel
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string ServicePlan { get; set; }
        public string VenueName { get; set; }
        public string TenantIdInString { get; set; }
        public string RecoveryState { get; set; }
        public System.DateTime LastUpdated { get; set; }
    }
}
