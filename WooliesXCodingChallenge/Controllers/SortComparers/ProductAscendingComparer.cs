using System;
using System.Collections.Generic;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Controllers
{
    class ProductAscendingComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            return StringComparer.Ordinal.Compare(x.Name, y.Name);
        }
    }
}
