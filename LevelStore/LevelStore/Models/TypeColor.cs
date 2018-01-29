using System.Collections.Generic;

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
