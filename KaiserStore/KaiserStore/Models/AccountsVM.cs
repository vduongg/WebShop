using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaiserStore.Models
{
    public class AccountsVM
    {
        [Key]
        [Required(ErrorMessage = "Tài khoản còn trống!")]
        [RegularExpression(@"^[A-Za-z 0-9]*$", ErrorMessage = "Không được sử dụng ký tự đặc biệt!")]
        [MaxLength(20, ErrorMessage ="Chỉ được sử dụng dưới 20 ký tự")]
        public string username { get; set; }
        [Required(ErrorMessage = "Email còn trống!")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage ="Điền đúng định dạng email!")]
        public string email { get; set; }
        [Required(ErrorMessage = "Tên còn trống!")]
        public string name { get; set; }
        [Required(ErrorMessage = "Mật khẩu còn trống!")]
        [MaxLength(20, ErrorMessage = "Chỉ được sử dụng dưới 20 ký tự")]
        public string password { get; set; }
        public string role{ get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Mật khẩu nhập lại còn trống!")]
        [Compare("password", ErrorMessage ="Không giống mật khẩu đã nhập!")]
        public string repeatpass { get; set; }

        public  List<Payment>? payments { get; set; } 
        public  List<Cart>? carts { get; set; }
    }
}
