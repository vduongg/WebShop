using System.ComponentModel.DataAnnotations;

namespace KaiserStore.Models
{
    public class CategoryVM
    {

        [Key]
        public string id { get; set; }
        public string namecategory { get; set; }
        public string active { get; set; }
    }
}
