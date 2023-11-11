using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class ProductsVM
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string producdName { get; set; }
        public int producdPrice { get; set; }
        [MaxLength]
        public byte[]? dataimage { get; set; }
        public string status { get; set; }
        public List<SizeItem>? sizes { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }
        public string describe { get; set; }
        public int sold { get; set; } = 0;
        [NotMapped]
        public IFormFile file { get; set; }
        public List<Cart>? carts { get; set; }
        public List<Order>? orders { get; set; }

    }
}
