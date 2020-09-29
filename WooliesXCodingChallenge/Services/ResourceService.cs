using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WooliesXCodingChallenge.Models;

namespace WooliesXCodingChallenge.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public ResourceService(IHttpClientFactory httpClientFactory, ILogger logger) {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            // Under normal circumstances I would put the token in the KeyVault, but Im getting issues trying to connect to it
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync("https://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products?token=fb0ffc0c-5a14-4a53-ba2b-d50e93c8fcf9");
            if (response.IsSuccessStatusCode)
            {
                try { 
                    return await response.Content.ReadAsAsync<List<Product>>();
                } catch (Exception e) {
                    throw new HttpRequestException(String.Format("Unable to parse products: {0}", e.Message));
                }
            }

            _logger.LogError(String.Format("Response from the products endpoint returned code: \"{0}\" with error: \"{1}\"", response.StatusCode, response.Content.ReadAsStringAsync()));
            throw new HttpRequestException();
        }

        public async Task<ActionResult<List<ShopperHistory>>> GetShopperHistory()
        {
            // Under normal circumstances I would put the token in the KeyVault, but Im getting issues trying to connect to it
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync("https://dev-wooliesx-recruitment.azurewebsites.net/api/resource/shopperHistory?token=fb0ffc0c-5a14-4a53-ba2b-d50e93c8fcf9");
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

            _logger.LogError(String.Format("Response from the shopperHistory endpoint returned code: \"{0}\" with error: \"{1}\"", response.StatusCode, response.Content.ReadAsStringAsync()));
            throw new HttpRequestException();
        }
    }
}
