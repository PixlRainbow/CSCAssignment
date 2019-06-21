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
        public IEnumerable<Product> GetAllProductsV1() {
            return products;
        }

        [HttpGet]
        [MapToApiVersion("2")]
        [MapToApiVersion("3")]
        public IEnumerable<Product> GetAllProductsV2V3(string category) {
            //action overloading by parameter was removed in dotnet core
            if (category == null)
                return repository.GetAll();
            else
                return GetProductsByCategory(category);
        }

        [HttpGet("{id:int:min(2)}")]
        [MapToApiVersion("1")]
        public IActionResult GetProductV1(int id){
            var product = products.FirstOrDefault(p => p.Id == id);
            if(product == null) return NotFound();
            else return Ok(product);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [MapToApiVersion("2")]
        [MapToApiVersion("3")]
        public IActionResult GetProductV2V3(int id){
            Product item = repository.Get(id);
            if(item == null) return NotFound();
            else return Ok(item);
        }

        public IEnumerable<Product> GetProductsByCategory(string category){
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase)
            );
        }

        [HttpPost]
        [MapToApiVersion("2")]
        public IActionResult PostProductV2(Product item){
            item = repository.Add(item);
            return CreatedAtRoute(
                "GetProduct",
                new {id = item.Id},
                item
            );
        }

        [HttpPost]
        [MapToApiVersion("3")]
        public IActionResult PostProductV3(Product.V3 item){
            if(ModelState.IsValid){
                item = (Product.V3)repository.Add(item);
                return CreatedAtRoute(
                    "GetProduct",
                    new {id = item.Id},
                    item
                );
            }else{
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        [MapToApiVersion("2")]
        [MapToApiVersion("3")]
        public IActionResult PutProduct(int id, Product product){
            product.Id = id;
            //NoContent is 204, "OK but nothing to say"
            if(repository.Update(product)) return NoContent();
            else return NotFound();
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("2")]
        [MapToApiVersion("3")]
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