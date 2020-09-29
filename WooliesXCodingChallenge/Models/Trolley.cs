using System.Collections.Generic;

namespace WooliesXCodingChallenge.Models
{
    public class Trolley
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<Product> Quantities { get; set; }
    }
}
