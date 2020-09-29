using System;
using System.Collections.Generic;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Controllers
{
    class ProductPopularityComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x.Popularity > y.Popularity)
            {
                return -1;
            }
            else if (x.Popularity < y.Popularity)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
