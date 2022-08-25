namespace CatalogService.Interfaces.IRepositories
{
    public interface IRepository<TModel>
    {
        TModel GetById(int id);
        TModel Create(TModel model);
        TModel Update(TModel model);
        bool Delete(int id);
    }
}
