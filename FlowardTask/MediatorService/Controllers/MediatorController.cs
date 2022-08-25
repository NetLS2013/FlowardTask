using Microsoft.AspNetCore.Mvc;
using SharedDto.DtoModels.ResponseDtoModels;
using SharedDto.Models;
using System.Text.Json;

namespace MediatorService.Controllers
{
    // TODO: finish CRUD for API (not needed full for demo here)
    [ApiController]
    [Route("[controller]/[action]")]
    public class MediatorController : ControllerBase
    {
        private HttpClient HttpClientInstance { get; }
        private string CatalogServiceUrl { get; }

        public MediatorController(IConfiguration config)
        {
            HttpClientInstance = new HttpClient();
            CatalogServiceUrl = config.GetValue<string>("CatalogServiceUrl");
        }

        [HttpPost]
        public async Task<string> AddProduct(ProductDto product)
        {
            ProductResponse response = new ProductResponse();

            if (ModelState.IsValid)
            {
                JsonContent content = JsonContent.Create(product);
                HttpResponseMessage addProductResponse = await HttpClientInstance.PostAsync($"{CatalogServiceUrl}/Products/Create", content);
                string responseString = await addProductResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    response = JsonSerializer.Deserialize<ProductResponse>(responseString);
                }
            }

            return JsonSerializer.Serialize(response);
        }
    }
}