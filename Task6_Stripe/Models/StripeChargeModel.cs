using System.ComponentModel.DataAnnotations;

namespace CSCAssignment.Models
{
    public class StripeChargeModel
    {
        [Required]
        public string Token { get; set; }
        
        [Required]
        public double Amount { get; set; }

        public string CardHolderName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostcode { get; set; }
        public string AddressCountry { get; set; }
    }
}