using System.Linq;

namespace Northwind.Models
{
    public class EFNorthwindRepository : INorthwindRepository
    {
        private NorthwindContext context;
        public EFNorthwindRepository(NorthwindContext ctx)
        {
            context = ctx;
        }
        
        public IQueryable<Category> Categories => context.Categories;
        public IQueryable<Product> Products => context.Products;

    }
}
