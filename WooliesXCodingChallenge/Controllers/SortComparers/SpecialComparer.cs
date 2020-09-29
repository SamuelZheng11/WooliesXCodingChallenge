using System;
using System.Collections.Generic;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Controllers
{
    class SpecialComparer : IComparer<Special>
    {
        public int Compare(Special x, Special y)
        {
            if (x.TotalSavings > y.TotalSavings)
            {
                return -1;
            }
            else if (x.TotalSavings < y.TotalSavings)
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
