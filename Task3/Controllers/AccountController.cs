using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CSCAssignment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSCAssignment.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel newUser){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new IdentityUser{UserName = newUser.Email, Email = newUser.Email};
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if(result.Succeeded){
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));

                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(){
            //clears cookies, but does not dereference token
            //library author has not yet implemented dereference methods
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}