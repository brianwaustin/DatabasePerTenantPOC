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
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;


        public EditModel(AppDbContext db, ICustomerRepository customerRepository, UserManager<AppUser> userManager)
        {
            _db = db;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public CustomerModel Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _customerRepository.GetCustomerById(id, _userManager.FindByNameAsync(User.Identity.Name).Result.TenantId);
            if (Customer == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _customerRepository.UpdateCustomer(Customer, _userManager.FindByNameAsync(User.Identity.Name).Result.TenantId);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Customer { Customer.Id} not found!", e);
            }

            return RedirectToPage("./Index");
        }
    }
}