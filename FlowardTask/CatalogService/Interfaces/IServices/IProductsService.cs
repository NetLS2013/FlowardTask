using CatalogService.DbModels;
using SharedDto.Models;

namespace CatalogService.Interfaces.IServices
{
    public interface IProductsService
    {
        ProductDto GetById(int productId);
        ProductDto Create(ProductDto productDtoModel);
        ProductDto Update(ProductDto productDbModel);
        bool Delete(int id);
    }
}
