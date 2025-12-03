using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace E_Handel.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; } = null!;

        public List<Product> Products { get; set; } = null!;
    }
}
