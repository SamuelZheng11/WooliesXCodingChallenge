using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Services
{
    public interface IResourceService
    {
        Task<ActionResult<List<Product>>> GetProducts();
        Task<ActionResult<List<ShopperHistory>>> GetShopperHistory();
    }
}
