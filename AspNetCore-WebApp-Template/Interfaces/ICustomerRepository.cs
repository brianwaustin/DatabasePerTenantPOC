using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> GetCustomerById(int customerId, int tenantId);

        Task<int> AddCustomer(CustomerModel customeModel, int tenantId);

        Task<List<CustomerModel>> GetCustomers(int tenantId);

        Task<bool> DeleteCustomerById(int customerId, int tenantId);
        Task<bool> UpdateCustomer(CustomerModel customer, int tenantId);
    }
}
