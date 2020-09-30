using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WooliesXCodingChallenge.Models;
using Microsoft.Extensions.Logging;

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
        public async Task<ActionResult<decimal>> GetTrolleyTotal([FromBody] Trolley trolley)
        {
            if (trolley.Products == null || trolley.Quantities == null || trolley.Specials == null) {
                return StatusCode(400);
            }
            // use a dictionary for constant access of product prices & number of items left in the cart
            decimal minimumTrolleyCost = 0;
            // variable used to quickly identify if there are any items remaining in the trolley (small optimisation for when a trolley only has specials in it)
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
                // perform resuffle of the savings on the speicals in the event that it affects the lowest trolley total
                minimumTrolleyCost += specialToApply.Total;
                specialToApply = GetSpecialToApply(trolley.Specials, itemsRemainingInCart);
            }


            // We cannot apply anymore specials to the cart so we need to add up the regular prices of any remaining items in the cart
            if (totalRemainingItemsInCart != 0)
            {
                foreach (KeyValuePair<string, float> product in itemsRemainingInCart.ToArray()) 
                {
                    if (product.Value > 0) 
                    {
                        minimumTrolleyCost += costForProducts[product.Key] * (decimal)product.Value;
                    }
                }
            }

            return await Task.FromResult(minimumTrolleyCost);
        }

        // determine how much each you save buy applying a special to your trolley, this is the core part of the algorithm as it helps determine if one should apply the special
        private decimal GetSpecialSavings(Special special, Dictionary<string, decimal> costForProducts) 
        {
            decimal costWithoutSpecial = 0;

            foreach (Product product in special.Quantities) 
            {
                costWithoutSpecial += costForProducts[product.Name] * (decimal)product.Quantity;
            }

            return costWithoutSpecial - special.Total;
        }

        // determine which special should be applied if no specials can be applied it will return null signaling that we cannot apply any more specials
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

        // determine if a special can be applied with the remaining items in the trolley
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
