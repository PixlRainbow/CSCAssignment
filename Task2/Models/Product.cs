using System;
using System.ComponentModel.DataAnnotations;

namespace CSCAssignment.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public class V3 : Product{
            [Required]
            public new string Name { get; set; }
            [Required]
            [Range(0,100)]
            public new decimal Price { get; set; }
        }
    }
}