using System.ComponentModel.DataAnnotations;

namespace KaiserStore.Models
{
    public class AccountsVM
    {
        [Key]
        public string username { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string role{ get; set; }
    }
}
