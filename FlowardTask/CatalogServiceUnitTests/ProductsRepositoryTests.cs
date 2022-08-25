using CatalogService.Migrations;
using CatalogService.DbModels;
using CatalogService.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CatalogServiceUnitTests
{
    public class ProductsRepositoryTests
    {
        private IConfiguration Config { get; }
        private string ConnectionString { get; }

        public ProductsRepositoryTests()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            ConnectionString = Config.GetConnectionString("DefaultConnection");
        }

        [Fact]
        public void TestGetById()
        {
            #region Get 1 product from DB
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string command = "SELECT TOP (1) * FROM [dbo].[Products]";
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            DataTable dataTable = new DataTable();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            dataTable.Load(reader);

            DataRow productDataRow = dataTable.Rows[0]; // requires some data in DB for test to work

            connection.Close();
            #endregion

            var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer(ConnectionString)
                .Options;
            CatalogContext catalogContext = new CatalogContext(dbOptions);
            ProductsRepository productsRepository = new ProductsRepository(catalogContext);

            int id = int.Parse(productDataRow["Id"].ToString());
            ProductDbModel product = productsRepository.GetById(id);

            Assert.NotNull(product);
            Assert.Equal(id, product.Id);
        }
    }
}