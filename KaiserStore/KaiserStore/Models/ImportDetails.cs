using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class ImportDetails
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public int quantity { get; set; }
        public DateTime DateTime { get; set; }
    }
}
