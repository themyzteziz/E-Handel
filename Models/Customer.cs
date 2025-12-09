using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Handel.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string? Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string? City { get; set; } = null!;

        public List<Order> Orders { get; set; } = null!;
    }
}
