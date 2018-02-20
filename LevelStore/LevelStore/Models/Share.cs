using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class Share
    {
        public int ShareId { get; set; }
        public List<Product> Products { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateOfStart { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateOfEnd { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        [Range(0, 100)]
        public double KoefPrice { get; set; }
        public bool Fake { get; set; }
    }
}
