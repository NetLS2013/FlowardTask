using CatalogService.HttpModels;
using CatalogService.Interfaces.IRepositories;
using CatalogService.Interfaces.IServices;
using CatalogService.DbModels;
using Microsoft.AspNetCore.Mvc;
using SharedDto.Models;
using System.Text.Json;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        private IProductsService ProductsServiceInstance { get; }
        private HttpClient HttpClientInstance { get; }
        private string MediatorServiceUrl { get; }

        public ProductsController(IConfiguration config, IProductsService productsService)
        {
            ProductsServiceInstance = productsService;
            HttpClientInstance = new HttpClient();
            MediatorServiceUrl = config.GetValue<string>("MediatorServiceUrl");
        }

        [HttpGet]
        public string Get(int productId)
        {
            ProductDto dtoModel = ProductsServiceInstance.GetById(productId);
            return JsonSerializer.Serialize(dtoModel);
        }

        [HttpPost]
        public string Create(ProductDto dtoModel)
        {
            ProductResponse response = new ProductResponse();

            if (ModelState.IsValid)
            {
                ProductDto dtoModelAdded = ProductsServiceInstance.Create(dtoModel);
                response.IsSuccess = dtoModelAdded != null;

                if (response.IsSuccess)
                {
                    response.Product = dtoModelAdded;
                    SendEmailAboutAddedProduct(dtoModelAdded);
                }
            }

            return JsonSerializer.Serialize(response);
        }

        private async Task SendEmailAboutAddedProduct(ProductDto dtoModel)
        {
            EmailDto email = new EmailDto()
            {
                Title = $"Product {dtoModel.Name} was added.",
                Body = $"Product {dtoModel.Name} was added."
            };
            JsonContent content = JsonContent.Create(email);
            HttpResponseMessage addProductResponse = await HttpClientInstance.PostAsync($"{MediatorServiceUrl}/Email/Send", content);
        }

        [HttpPatch]
        public string Update(ProductDto product)
        {
            BaseResponse response = new BaseResponse();

            if (ModelState.IsValid)
            {
                ProductDto addedProduct = ProductsServiceInstance.Update(product);
                response.IsSuccess = addedProduct != null;
            }

            return JsonSerializer.Serialize(response);
        }

        [HttpDelete]
        public string Delete(int id)
        {
            BaseResponse response = new BaseResponse();

            response.IsSuccess = ProductsServiceInstance.Delete(id);

            return JsonSerializer.Serialize(response);
        }
    }
}