using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Handel.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set;  }

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public decimal TotalAmount { get; set; }

        public Customer? Customer { get; set; } = null!;

        public List<OrderRow> OrderRows { get; set; } = null!;

    }
}
