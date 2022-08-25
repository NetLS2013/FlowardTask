using SharedDto.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SharedDto.Models
{
    public class ProductDto : IProduct
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        public string Image { get; set; }
    }
}
