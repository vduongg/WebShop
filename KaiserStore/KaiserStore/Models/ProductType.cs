using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class ProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }    
        public List<ProductsVM>? products { get; set; }
        public CategoryVM? category { get; set; }
        public string categoryId { get; set; }
        public string status { get; set; }
    }
}
