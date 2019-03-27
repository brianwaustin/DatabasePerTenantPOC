using DatabasePerTenantPOC.Data.CatalogDB;
using DatabasePerTenantPOC.Data.CustomerDB;
using DatabasePerTenantPOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Mapping
{
    public static class Mapper
    {
        #region Entity To Model Mapping

        public static TenantModel ToTenantModel(this Tenants tenantEntity)
        {
            string tenantIdInString = BitConverter.ToString(tenantEntity.TenantId);
            tenantIdInString = tenantIdInString.Replace("-", "");

            return new TenantModel
            {
                ServicePlan = tenantEntity.ServicePlan,
                TenantId = ConvertByteKeyIntoInt(tenantEntity.TenantId),
                TenantName = tenantEntity.TenantName,
                TenantIdInString = tenantIdInString,
                RecoveryState = tenantEntity.RecoveryState,
                LastUpdated = tenantEntity.LastUpdated
            };
        }

        public static CustomerModel ToCustomerModel(this Customer customer)
        {
            return new CustomerModel
            {
                Name = customer.Name,                
                Id = customer.Id
            };
        }

        public static Customer ToCustomersEntity(this CustomerModel customeModel)
        {
            return new Customer
            {
                Id = customeModel.Id,
                Name = customeModel.Name 
            };
        }        

        /// <summary>
        /// Converts the byte key into int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static int ConvertByteKeyIntoInt(byte[] key)
        {
            // Make a copy of the normalized array
            byte[] denormalized = new byte[key.Length];

            key.CopyTo(denormalized, 0);

            // Flip the last bit and cast it to an integer
            denormalized[0] ^= 0x80;

            return IPAddress.HostToNetworkOrder(BitConverter.ToInt32(denormalized, 0));
        }

        #endregion
    }
}
