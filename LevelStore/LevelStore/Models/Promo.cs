using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class Promo
    {
        public int PromoId { get; set; }
        [Required]
        public String PromoCode { get; set; }
        [Required]
        [Range(0, 20)]
        public int Discount { get; set; }
    }
}
