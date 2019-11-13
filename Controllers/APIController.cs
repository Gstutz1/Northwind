using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Controllers
{
    public class APIController : Controller
    {
        private readonly INorthwindRepository repository;

        public APIController(INorthwindRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet, Route("api/product")]
        public IEnumerable<Product> Get() => repository.Products.OrderBy(p => p.ProductId);

        [HttpGet, Route("api/product/{id}")]
        public Product Get(int id) => repository.Products.FirstOrDefault(p => p.ProductId == id);

        [HttpGet, Route("api/product/discontinued/{discontinued}")]
        public IEnumerable<Product> GetDiscontinued(bool discontinued) => repository.Products.Where(p =>
            p.Discontinued == discontinued).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product")]
        public IEnumerable<Product> GetByCategoryDiscontinued(int categoryId) => repository.Products.Where(
            p => p.CategoryId == categoryId).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product/discontinued/{discontinued}")]
        public IEnumerable<Product> GetByCategoryDiscontinued(int categoryId, bool discontinued) => 
            repository.Products.Where(p => p.CategoryId == categoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName);
    }
}