using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabasePerTenantPOC.Data.CustomerDB;
using DatabasePerTenantPOC.Data.TenantDB;
using DatabasePerTenantPOC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DatabasePerTenantPOC.Pages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;

        private ILogger<CreateModel> _log;

        public CreateModel(AppDbContext db, ICustomerRepository customerRepository, ILogger<CreateModel> log)
        {
            _db = db;
            _customerRepository = customerRepository;
            _log = log;
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

            await _customerRepository.AddCustomer(Customer, -1526297073);
            var msg = $"Customer {Customer.Name} added!";
            Message = msg;
            _log.LogCritical(msg);
            return RedirectToPage("/Index");
        }
    }
}