using DatabasePerTenantPOC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Interfaces
{
    public interface IUtilities
    {
        void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig, bool resetEventDate);

        byte[] ConvertIntKeyToBytesArray(int key);

        string GetTenantStatus(int TenantId);

        void ResolveMappingDifferences(int TenantId, bool UseGlobalShardMap = false);
    }
}
