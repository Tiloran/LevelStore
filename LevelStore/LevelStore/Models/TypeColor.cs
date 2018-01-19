using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class TypeColor
    {
        public int TypeColorID { get; set; }
        public string ColorType { get; set; }
        public List<Color> Color { get; set; }
        public List<Image> Images { get; set; }
    }
}
