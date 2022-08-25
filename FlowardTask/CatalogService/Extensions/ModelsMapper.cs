using CatalogService.DbModels;
using SharedDto.Models;

namespace CatalogService.Extensions
{
    public static class ModelsMapper
    {
        public static ProductDbModel ToDbModel(this ProductDto dtoModel)
        {
            return new ProductDbModel()
            {
                Id = dtoModel.Id,
                Name = dtoModel.Name,
                Price = dtoModel.Price,
                Cost = dtoModel.Cost,
                Image = dtoModel.Image
            };
        }

        public static ProductDto ToDtoModel(this ProductDbModel dbModel)
        {
            return new ProductDto()
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Price = dbModel.Price,
                Cost = dbModel.Cost,
                Image = dbModel.Image
            };
        }
    }
}
