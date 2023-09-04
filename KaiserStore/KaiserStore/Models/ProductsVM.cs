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
        [MaxLength(100)]
        public string producdPrice { get; set; }
        [MaxLength(100)]
        public string productType { get; set; }
        [MaxLength]
        public byte[] dataimage { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }

    }
}
