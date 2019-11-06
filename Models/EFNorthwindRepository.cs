using System.Linq;

namespace Northwind.Models
{
    public class EfNorthwindRepository : INorthwindRepository
    {
        private readonly NorthwindContext context;
        public EfNorthwindRepository(NorthwindContext context)
        {
            this.context = context;
        }
        
        public IQueryable<Category> Categories => context.Categories;
        public IQueryable<Product> Products => context.Products;
        public IQueryable<Discount> Discounts => context.Discounts;
        public IQueryable<Customer> Customers => context.Customers;

        public void AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void EditCustomer(Customer customer)
        {
            var customerToUpdate = context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (customerToUpdate != null)
            {
                customerToUpdate.Address = customer.Address;
                customerToUpdate.City = customer.City;
                customerToUpdate.Region = customer.Region;
                customerToUpdate.PostalCode = customer.PostalCode;
                customerToUpdate.Country = customer.Country;
                customerToUpdate.Phone = customer.Phone;
                customerToUpdate.Fax = customer.Fax;
            }

            context.SaveChanges();
        }
    }
}
