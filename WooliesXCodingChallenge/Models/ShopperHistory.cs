using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WooliesXCodingChallenge.Models
{
    public class ShopperHistory
    {
        public long CustomerID { get; set; }
        public List<Product> products { get; set; }
    }
}
