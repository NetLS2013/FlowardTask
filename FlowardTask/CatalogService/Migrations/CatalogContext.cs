using CatalogService.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Migrations
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options) { }

        public DbSet<ProductDbModel> Products { get; set; }
    }
}
