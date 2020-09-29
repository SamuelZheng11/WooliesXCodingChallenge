using System.Collections.Generic;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Controllers
{
    class ProductHighComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x.Price > y.Price)
            {
                return -1;
            }
            else if (x.Price < y.Price)
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
