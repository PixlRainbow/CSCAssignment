using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSCAssignment.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            var userName = this.User.Identity.Name;
            return String.Format("Hello, {0}.", userName);
        }
    }
}