using SharedDto.Models;


namespace CatalogService.HttpModels
{
    public class ProductResponse : BaseResponse
    {
        public ProductDto Product { get; set; }
    }
}
