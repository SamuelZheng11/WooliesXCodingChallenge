using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Services
{
    public interface IResourceQueryService
    {
        Task<ActionResult<List<Product>>> GetProducts();
        Task<ActionResult<List<ShopperHistory>>> GetShopperHistory();
    }
}
