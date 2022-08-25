using CatalogService.Interfaces.IRepositories;
using CatalogService.Interfaces.IServices;
using CatalogService.DbModels;
using SharedDto.Models;
using CatalogService.Extensions;

namespace CatalogService.Services
{
    public class ProductsService : IProductsService
    {
        private IRepository<ProductDbModel> Repository { get; }

        public ProductsService(IRepository<ProductDbModel> productsRepository)
        {
            Repository = productsRepository;
        }

        public ProductDto GetById(int productId)
        {
            return Repository.GetById(productId).ToDtoModel();
        }

        public ProductDto Create(ProductDto product)
        {
            return Repository.Create(
                                        product.ToDbModel()
                                    ).ToDtoModel();
        }

        public ProductDto Update(ProductDto product)
        {
            return Repository.Update(
                                        product.ToDbModel()
                                    ).ToDtoModel();
        }

        public bool Delete(int id)
        {
            return Repository.Delete(id);
        }
    }
}
