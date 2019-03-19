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
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ICustomerRepository _customerRepository;


        public EditModel(AppDbContext db, ICustomerRepository customerRepository) { _db = db; _customerRepository = customerRepository; }

        [BindProperty]
        public CustomerModel Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _customerRepository.GetCustomerById(id, -1526297073);
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
                await _customerRepository.UpdateCustomer(Customer, -1526297073);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Customer { Customer.Id} not found!", e);
            }

            return RedirectToPage("./Index");
        }
    }
}