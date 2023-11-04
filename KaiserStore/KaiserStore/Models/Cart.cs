using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AccountsVM User { get; set; }
        public string UserId { get; set; }
        public ProductsVM Product { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public int quantity { get; set; }
    }
}

