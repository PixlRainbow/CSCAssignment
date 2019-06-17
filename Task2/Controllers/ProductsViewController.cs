using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSCAssignment.Controllers
{
    [ControllerName("Products")]
    public class ProductsViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}