using DatabasePerTenantPOC.Data.CatalogDB;
using DatabasePerTenantPOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<TenantModel>> GetAllTenants();
        Task<TenantModel> GetTenant(string tenantName);
        bool Add(Tenants tenant);
    }
}
