using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using CSCAssignment.Models;

namespace CSCAssignment.Controllers
{
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [EnableCors("AllowLocalAndAWS")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>for V1 backwards compatibility</summary>
        Product[] products = new Product[]{
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };
        /// <summary>for V2</summary>
        static readonly IProductRepository repository = new ProductRepository();

        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion() {
            return Ok(new {
                version = GetAPIVersion()
            });
        }

        private string GetAPIVersion(){
            return HttpContext.GetRequestedApiVersion().ToString();
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("message")]
        public IActionResult GetMultipleNames(string name1, string name2, string name3){
            return Ok(new {
                name1 = name1,
                name2 = name2,
                name3 = name3
            });
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [MapToApiVersion("2")]
        public IEnumerable<Product> GetAllProducts(string category) {
            switch (GetAPIVersion())
            {
                case "1":
                    return products;
                case "2":
                    //action overloading was removed in dotnet core
                    if (category == null)
                        return repository.GetAll();
                    else
                        return GetProductsByCategory(category);
                default:
                    // this code is never reached.
                    // It is just used to suppress compile error
                    return null;
            }
        }

        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [MapToApiVersion("2")]
        public IActionResult GetProduct(int id){
            switch (GetAPIVersion())
            {
                case "1":
                    //replaces attribute validation for min(2)
                    //this is needed as V2 does not have this restriction
                    if(id < 2) return NotFound();

                    var product = products.FirstOrDefault(p => p.Id == id);
                    if(product == null) return NotFound();
                    else return Ok(product);
                case "2":
                    Product item = repository.Get(id);
                    if(item == null) return NotFound();
                    else return Ok(item);
                default:
                    return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category){
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase)
            );
        }

        [HttpPost]
        [MapToApiVersion("2")]
        public IActionResult PostProduct(Product item){
            item = repository.Add(item);
            return CreatedAtAction(
                nameof(GetProduct),
                new {id = item.Id},
                item
            );
        }

        [HttpPut("{id}")]
        [MapToApiVersion("2")]
        public IActionResult PutProduct(int id, Product product){
            product.Id = id;
            //NoContent is 204, "OK but nothing to say"
            if(repository.Update(product)) return NoContent();
            else return NotFound();
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("2")]
        public IActionResult DeleteProduct(int id){
            Product item = repository.Get(id);
            if(item == null) return NotFound();
            else {
                repository.Remove(id);
                return NoContent();
            }
        }
    }
}