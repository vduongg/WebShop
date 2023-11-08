using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public ProductsVM? Product { get; set; }
        public int ProductId { get; set; }
        public Payment? Payment { get; set; }
        public int PaymentId { get; set; }
        public string Size { get; set; }
        public int quantity { get; set; }


    }
}
