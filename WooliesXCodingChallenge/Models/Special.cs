using System.Collections.Generic;

namespace WooliesXCodingChallenge.Models
{
    public class Special
    {
        public List<Product> Quantities { get; set; }
        public decimal Total { get; set; }
        public decimal? TotalSavings { get; set; }
    }
}
