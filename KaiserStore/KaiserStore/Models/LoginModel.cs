using System.ComponentModel.DataAnnotations;

namespace KaiserStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Tài khoản còn trống!")]
        public string user { get; set; }
        [Required(ErrorMessage = "Mật khẩu còn trống!")]
        public string pass { get; set; }
 
    }
}
