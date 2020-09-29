using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WooliesXCodingChallenge.Models
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public float Quantity { get; set; }
        public long? Popularity { get; set; }
    }
}
