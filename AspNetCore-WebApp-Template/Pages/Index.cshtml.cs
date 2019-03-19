using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabasePerTenantPOC.Data.CustomerDB;
using DatabasePerTenantPOC.Data.TenantDB;
using DatabasePerTenantPOC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DatabasePerTenantPOC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;

        public IndexModel(AppDbContext db, ICustomerRepository customerRepository) { _db = db; _customerRepository = customerRepository; }

        public IList<CustomerModel> Customers { get; private set; }

        [TempData]
        public string Message { get; set; }
    
        public async Task OnGetAsync()
        {            
            Customers = await _customerRepository.GetCustomers(-1526297073);            
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {            
            await _customerRepository.DeleteCustomerById(id, -1526297073);
            
            return RedirectToPage();
        }
    }
}
