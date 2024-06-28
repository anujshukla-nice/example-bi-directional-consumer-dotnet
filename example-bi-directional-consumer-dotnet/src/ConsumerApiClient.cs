using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Provider.Models;

namespace Consumer
{
    public class ProductClient
    {
        #nullable enable
        public async Task<List<Product>> GetProducts(string baseUrl, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "Products");
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(responseStr);
        }


        public async Task<Product> GetProduct(string baseUrl, int productId, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "Products/" + productId);
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(resp);
        }

        public async Task<Product> AddProducts(string baseUrl, Product product, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var content = JsonConvert.SerializeObject(product);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl + "Products");
            if (!string.IsNullOrEmpty(content))
            {
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(httpRequestMessage);
            var responseProduct = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(responseProduct) ?? new Product();
        }
    }
}