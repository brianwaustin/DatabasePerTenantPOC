using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabasePerTenantPOC.Data;
using DatabasePerTenantPOC.Data.CustomerDB;
using DatabasePerTenantPOC.Data.TenantDB;
using DatabasePerTenantPOC.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DatabasePerTenantPOC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(AppDbContext db, ICustomerRepository customerRepository, UserManager<AppUser> userManager)
        {
            _db = db;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        public IList<CustomerModel> Customers { get; private set; }

        [TempData]
        public string Message { get; set; }
    
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;            

                Customers = await _customerRepository.GetCustomers(user.TenantId);  
            }
            else
            {
                Message = "Please Log In to Add Data";
            }            
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {            
            await _customerRepository.DeleteCustomerById(id, _userManager.FindByNameAsync(User.Identity.Name).Result.TenantId);
            
            return RedirectToPage();
        }
    }
}
