using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
    public class CustomerWithPassword
    {
        public Customer Customer { get; set; }
        [UIHint("password"), Required]
        public string Password { get; set; }
    }
}
