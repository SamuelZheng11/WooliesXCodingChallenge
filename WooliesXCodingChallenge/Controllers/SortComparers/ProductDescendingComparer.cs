using System;
using System.Collections.Generic;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Controllers
{
    class ProductDescendingComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            return (-1) * StringComparer.Ordinal.Compare(x.Name, y.Name);
        }
    }
}
