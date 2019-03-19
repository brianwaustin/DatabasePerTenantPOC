using DatabasePerTenantPOC.Data.TenantDB;
using DatabasePerTenantPOC.Interfaces;
using DatabasePerTenantPOC.Utilities;
using DatabasePerTenantPOC.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatabasePerTenantPOC.Data.CustomerDB;

namespace DatabasePerTenantPOC.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public virtual DbSet<Customer> Customers { get; set; }        

        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private CustomerDbContext CreateContext(int tenantId)
        {
            return new CustomerDbContext(Sharding.ShardMap, tenantId, _connectionString);
        }

        public async Task<int> AddCustomer(CustomerModel customeModel, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {               
                var customer = customeModel.ToCustomersEntity();

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                return customer.Id;
            }
        }

        public async Task<CustomerModel> GetCustomerById(int customerId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var customer = await context.Customers.FirstOrDefaultAsync(i => i.Id == customerId);

                return customer?.ToCustomerModel();
            }
        }

        public async Task<List<CustomerModel>> GetCustomers(int tenantId)
        {
            var results = new List<CustomerModel>();

            using (var context = CreateContext(tenantId))
            {
                var Customers2 = await context.Customers.ToListAsync<Customer>();

                foreach(Customer customer in Customers2)
                {                    
                    results.Add(customer?.ToCustomerModel());
                }

                return results;
            }
        }        
        
        public async Task<bool> DeleteCustomerById(int customerId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var customer = context.Customers.FirstOrDefaultAsync(i => i.Id == customerId);

                if(customer != null)
                {
                    context.Customers.Remove(customer.Result);

                    return (await context.SaveChangesAsync() > 0);                   
                }

                return false;
            }           
        }

        public async Task<bool> UpdateCustomer(CustomerModel customer, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                context.Attach(customer?.ToCustomersEntity()).State = EntityState.Modified;

                return (await context.SaveChangesAsync() > 0);                
            }
            
        }
    }
}
