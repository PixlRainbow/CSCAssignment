using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSCAssignment.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ValuesController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        // GET api/values
        [HttpGet]
        public string Get()
        {
            var sub = this.User.Claims
                .Where(c => c.Type.Equals("sub"))
                .FirstOrDefault();
            if(sub != null){
                string userId = sub.Value;
                var user = _userManager.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
                if(user != null){
                    string userName = user.UserName;
                    return String.Format("Hello, {0}.", userName);
                }
                return "cannot find user with given id";
            }
            return "no user id associated with token";
        }
    }
}