using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.DbModels
{
    [Table("Products")]
    public class ProductDbModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        public string Image { get; set; }
    }
}
