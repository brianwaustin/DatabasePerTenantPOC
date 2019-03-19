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
using Microsoft.Extensions.Logging;

namespace DatabasePerTenantPOC.Pages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        private ILogger<CreateModel> _log;

        public CreateModel(AppDbContext db, ICustomerRepository customerRepository, UserManager<AppUser> userManager, ILogger<CreateModel> log)
        {
            _db = db;
            _customerRepository = customerRepository;
            _log = log;
            _userManager = userManager;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public CustomerModel Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _customerRepository.AddCustomer(Customer, _userManager.FindByNameAsync(User.Identity.Name).Result.TenantId);
            var msg = $"Customer {Customer.Name} added!";
            Message = msg;
            _log.LogCritical(msg);
            return RedirectToPage("/Index");
        }
    }
}