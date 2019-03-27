using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenantPOC.Data.CustomerDB
{
    public class Customer
    {
        public Customer() {}

        public int Id { get; set; }
        
        [Required, StringLength(10)]
        public string Name { get; set; }
        
    }
}
