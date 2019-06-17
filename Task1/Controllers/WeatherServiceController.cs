using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CSCAssignment.Controllers
{
    public class WeatherServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult JQuery(){
            return View();
        }
    }
}