using CatalogService.Interfaces.IRepositories;
using CatalogService.Migrations;
using CatalogService.DbModels;

namespace CatalogService.Repositories
{
    public class ProductsRepository : IRepository<ProductDbModel>
    {
        private CatalogContext Context { get; }

        public ProductsRepository(CatalogContext context)
        {
            Context = context;
        }

        public ProductDbModel GetById(int id)
        {
            return Context.Products.FirstOrDefault(product => product.Id == id);
        }

        public ProductDbModel Create(ProductDbModel product)
        {
            ProductDbModel resultProduct = Context.Products.Add(product).Entity;
            Context.SaveChanges();
            return resultProduct;
        }

        public ProductDbModel Update(ProductDbModel product)
        {
            ProductDbModel resultProduct = Context.Products.Update(product).Entity;
            Context.SaveChanges();
            return resultProduct;
        }

        public bool Delete(int id)
        {
            Context.Products.Remove(GetById(id));
            return Context.SaveChanges() > 0;
        }
    }
}
