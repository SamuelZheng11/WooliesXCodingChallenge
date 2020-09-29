﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WooliesXCodingChallenge.Models
{
    public class Special
    {
        public List<Product> Quantities { get; set; }
        public decimal Total { get; set; }
        public decimal? TotalSavings { get; set; }
    }
}
