using SharedDto.Models;

namespace SharedDto.DtoModels.ResponseDtoModels
{
    public class ProductResponse : BaseResponse
    {
        public ProductDto Product { get; set; }
    }
}
