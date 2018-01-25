using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Alternative { get; set; }
        public int ProductID { get; set; }
        public int? TypeColorID { get; set; }
        public bool FirstOnScreen { get; set; }
        public bool SecondOnScreen { get; set; }

    }
}
