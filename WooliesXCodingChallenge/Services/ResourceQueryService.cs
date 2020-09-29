using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

namespace WooliesXCodingChallenge.Services
{
    public class ResourceQueryService : IResourceQueryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(String.Format("https://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products?token=fb0ffc0c-5a14-4a53-ba2b-d50e93c8fcf9", this.GetSecretToken()));
            if (response.IsSuccessStatusCode)
            {
                try { 
                    return await response.Content.ReadAsAsync<List<Product>>();
                } catch (Exception e) {
                    throw new HttpRequestException(String.Format("Unable to parse products: {0}", e.Message));
                }
            }
            throw new HttpRequestException(String.Format("Response from the products endpoint returned code: \"{0}\" with error: \"{1}\"", response.StatusCode, response.Content.ReadAsStringAsync()));
        }

        public async Task<ActionResult<List<ShopperHistory>>> GetShopperHistory()
        {
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(String.Format("https://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products?token=fb0ffc0c-5a14-4a53-ba2b-d50e93c8fcf9", this.GetSecretToken()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadAsAsync<List<ShopperHistory>>();
                }
                catch (Exception e)
                {
                    throw new HttpRequestException(String.Format("Unable to parse shopper history: {0}", e.Message));
                }
            }

            throw new HttpRequestException(String.Format("Response from the shopperHistory endpoint returned code: \"{0}\" with error: \"{1}\"", response.StatusCode, response.Content.ReadAsStringAsync()));
        }
    }
}
