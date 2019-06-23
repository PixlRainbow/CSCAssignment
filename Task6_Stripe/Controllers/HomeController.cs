using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSCAssignment.Models;
using Stripe;

namespace CSCAssignment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Charge(){
            ViewBag.Message = "Learn how to process payments with Stripe";
            return View(new StripeChargeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Charge(StripeChargeModel model){
            if(!ModelState.IsValid){
                return View(model);
            }
            var chargeId = await ProcessPayment(model);
            return View("Index");
        }

        private async Task<string> ProcessPayment(StripeChargeModel model){
            return await Task.Run(() => {
                var myCharge = new ChargeCreateOptions{
                    Amount = (int)(model.Amount * 100),
                    Currency = "gbp",
                    Description = "Description for test charge",
                    Source = model.Token
                };

                //Not required to specify secret key since set globally
                var chargeService = new ChargeService();
                var stripeCharge = chargeService.Create(myCharge);

                return stripeCharge.Id;
            });
        }
    }
}
