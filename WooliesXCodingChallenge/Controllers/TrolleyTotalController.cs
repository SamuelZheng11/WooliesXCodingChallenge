using System;
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
    [Route("api/trolleyTotal")]
    [ApiController]
    public class TrolleyTotalController : ControllerBase
    {

        // I will document my thought process here, I believe that is problem will require an `interpreter` like pattern
        // I can see that there are specials added to a trolley and a limited number of purchases for each item, my initial thoughts was to implement more of a `greedy` algoritm
        // First process the savings I can make from each special (IE if I have 2 cans of coke and there is a 2 for 1 deal I would save $1 if I used the special)
        // I would then use the savings to calculate an order of which speicals I should apply first and work my way through that ordering comparing which savings to apply, decrementing
        // the product count each time I apply a savings

        // On second pass I realised that in cases where the sum of less savings specials can sometimes be greater than the most cost savings special therefore we need to sort the order of 
        // savings each time
        [HttpPost]
        public decimal GetTrolleyTotal([FromBody] Trolley trolley)
        {
            // use a dictionary for constant access of product prices & number of items left in the cart
            decimal minimumTrolleyCost = 0;
            float totalRemainingItemsInCart = 0;
            trolley.Quantities.ForEach(product => {
                totalRemainingItemsInCart += product.Quantity;
            });
            Dictionary<string, decimal> costForProducts = trolley.Products.ToDictionary(product => product.Name, product => product.Price);
            Dictionary<string, float> itemsRemainingInCart = trolley.Quantities.ToDictionary(product => product.Name, product => product.Quantity);

            // Add up the speical prices and decrement the counters for items in cart
            SortSpecials(trolley.Specials, costForProducts);
            Special specialToApply = GetSpecialToApply(trolley.Specials, itemsRemainingInCart);
            while (specialToApply != null)
            {
                foreach (Product product in specialToApply.Quantities)
                {
                    totalRemainingItemsInCart -= product.Quantity;
                    itemsRemainingInCart[product.Name] -= product.Quantity;
                }
                minimumTrolleyCost += specialToApply.Total;
                SortSpecials(trolley.Specials, costForProducts);
                specialToApply = GetSpecialToApply(trolley.Specials, itemsRemainingInCart);
            }

            if (totalRemainingItemsInCart != 0)
            {
                foreach (KeyValuePair<string, float> product in itemsRemainingInCart.ToArray()) 
                {
                    if (product.Value > 0) 
                    {
                        minimumTrolleyCost += (decimal)costForProducts[product.Key] * (decimal)product.Value;
                    }
                }
            }
            return minimumTrolleyCost;
        }

        private decimal GetSpecialSavings(Special special, Dictionary<string, decimal> costForProducts) 
        {
            decimal costWithoutSpecial = 0;

            foreach (Product product in special.Quantities) 
            {
                costWithoutSpecial += costForProducts[product.Name] * (decimal)product.Quantity;
            }

            return costWithoutSpecial - special.Total;
        }

        private Special GetSpecialToApply(List<Special> specials, Dictionary<string, float> itemsRemainingInCart) 
        {
            foreach (Special special in specials)
            {
                if (CanApplySpecial(special, itemsRemainingInCart)) {
                    return special;
                }
            }
            return null;
        }

        private bool CanApplySpecial(Special special, Dictionary<string, float> itemsRemainingInCart) 
        {
            foreach (Product product in special.Quantities)
            {
                if (itemsRemainingInCart[product.Name] < product.Quantity) 
                {
                    return false;
                }
            }
            return true;
        }

        // sort by savings with most savings first
        private void SortSpecials(List<Special> specials, Dictionary<string, decimal> costForProducts) 
        {
            foreach (Special special in specials)
            {
                special.TotalSavings = GetSpecialSavings(special, costForProducts);
            }
            specials.Sort(new SpecialComparer());
        }
    }
}
