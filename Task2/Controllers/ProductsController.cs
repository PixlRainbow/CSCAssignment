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
        Product[] products = new Product[]{
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion() {
            return Ok(new {
                version = HttpContext.GetRequestedApiVersion().ToString()
            });
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
        public IEnumerable<Product> GetAllProducts() {
            return products;
        }

        [HttpGet("{id:int:min(2)}")]
        [MapToApiVersion("1")]
        public IActionResult GetProduct(int id){
            var product = products.FirstOrDefault(p => p.Id == id);
            if(product == null) return NotFound();
            else return Ok(product);
        }
    }
}