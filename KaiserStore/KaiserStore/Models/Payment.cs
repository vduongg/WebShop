using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        public AccountsVM? User { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public int? Total { get; set; }
        public int TotalPrice { get; set; }
        public string status { get; set; }
        public DateTime? DateTime { get; set; }
        public List<Order>? Orders { get; set; }
  
    }
}
