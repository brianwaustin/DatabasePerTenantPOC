using System.ComponentModel.DataAnnotations;

namespace DatabasePerTenantPOC
{
    public class CustomerModel
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Name { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PostalCode { get; set; }

        public string CountryCode { get; set; }

        public string TenantName { get; set; }

    }
}