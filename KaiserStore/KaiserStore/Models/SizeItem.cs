using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class SizeItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get; set; }

        public string Name { get; set; }
        public ProductsVM? Product { get; set; }
      
        public int ProductId { get; set; }
        public int Quantity { get; set; } 

    }
}
