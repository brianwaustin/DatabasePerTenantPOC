using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Data.CustomerDB
{
    public class Customer
    {
        public Customer()
        {
            //TicketPurchases = new HashSet<TicketPurchases>();
        }

        public int Id { get; set; }
        //public int CustomerId { get; set; }
        [Required, StringLength(10)]
        public string Name { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string PostalCode { get; set; }
        //public string CountryCode { get; set; }
        //public byte[] RowVersion { get; set; }

        //public virtual ICollection<TicketPurchases> TicketPurchases { get; set; }
        //public virtual Countries CountryCodeNavigation { get; set; }
    }
}
