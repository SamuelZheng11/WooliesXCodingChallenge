using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WooliesXCodingChallenge.Models;
using WooliesXCodingChallenge.Services;

namespace WooliesXCodingChallenge.Controllers
{
    [Route("api/sort")]
    [ApiController]
    public class SortController : ControllerBase
    {
        private readonly IResourceQueryService _resourceQueryService;

        public SortController(IResourceQueryService resourceQueryService)
        {
            _resourceQueryService = resourceQueryService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IList<Product>> GetProducts([FromQuery] string sortOption)
        {
            List<Product> products = (await _resourceQueryService.GetProducts()).Value;
            IComparer<Product> comparer = null;

            switch (sortOption) {
                case "Low":
                    comparer = new ProductLowComparer();
                    break;

                case "High":
                    comparer = new ProductHighComparer();
                    break;

                case "Ascending":
                    comparer = new ProductAscendingComparer();
                    break;

                case "Descending":
                    comparer = new ProductDescendingComparer();
                    break;

                case "Recommended":
                    List<ShopperHistory> shoppingHistories = (await _resourceQueryService.GetShopperHistory()).Value;
                    Dictionary<string, int> productDictionary = products.ToDictionary(x => x.Name, x => 0);
                    foreach (ShopperHistory history in shoppingHistories)
                    {
                        foreach(Product product in history.products)
                        {
                            productDictionary[product.Name] += 1;                            
                        }
                    }

                    products.Clear();
                    foreach (KeyValuePair<string, int> keypair in productDictionary)
                    {
                        products.Add(new Product() { 
                            Name = keypair.Key,
                            Popularity = keypair.Value
                        });
                    }

                    comparer = new ProductPopularityComparer();
                    break;

                default:
                    throw new NotImplementedException();
            }
            products.Sort(comparer);

            return await Task.FromResult(products);
        }
    }
}
