using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WooliesXCodingChallenge.Models;
using WooliesXCodingChallenge.Services;

namespace WooliesXCodingChallenge.Controllers
{
    [Route("api/sort")]
    [ApiController]
    public class SortController : ControllerBase
    {
        private readonly IResourceService _resourceQueryService;
        private readonly ILogger _logger;

        public SortController(IResourceService resourceQueryService, ILogger logger)
        {
            _resourceQueryService = resourceQueryService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IList<Product>> GetProducts([FromQuery] string sortOption)
        {
            List<Product> products = (await _resourceQueryService.GetProducts()).Value;
            IComparer<Product> comparer = null;
            _logger.LogInformation(String.Format("Requeted to sort by {0}", sortOption));
            switch (sortOption) {
                case "Low":
                    comparer = new ProductLowComparer();
                    _logger.LogDebug(String.Format("Sorting by Low with Products: {0} ", products));
                    break;

                case "High":
                    comparer = new ProductHighComparer();
                    _logger.LogDebug(String.Format("Sorting by High with Products: {0} ", products));
                    break;

                case "Ascending":
                    comparer = new ProductAscendingComparer();
                    _logger.LogDebug(String.Format("Sorting by Ascending with Products: {0} ", products));
                    break;

                case "Descending":
                    comparer = new ProductDescendingComparer();
                    _logger.LogDebug(String.Format("Sorting by Descending with Products: {0} ", products));
                    break;

                case "Recommended":
                    // organise into hashmap/dictionary for quick access of the `popularity` property on the product object
                    List<ShopperHistory> shoppingHistories = (await _resourceQueryService.GetShopperHistory()).Value;
                    // the use of a long here in the event we are looking at thousands of customer orders and the number of orders exceeds the limit for int
                    Dictionary<string, long> productDictionary = products.ToDictionary(product => product.Name, _ => (long) 0);

                    _logger.LogDebug(String.Format("Sorting by Recommended with ShopperHistory: \"{0}\" and Products: \"{1}\"", shoppingHistories, products));
                    foreach (ShopperHistory history in shoppingHistories)
                    {
                        foreach(Product product in history.products)
                        {
                            productDictionary[product.Name] += (long)product.Quantity;                            
                        }
                    }
                    
                    // Update optional popularity field for each product so that it can be sorted by that field
                    foreach (KeyValuePair<string, long> keypair in productDictionary)
                    {
                        products.Find(product => product.Name == keypair.Key).Popularity = keypair.Value;
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
