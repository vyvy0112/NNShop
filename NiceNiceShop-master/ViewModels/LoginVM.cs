using System.ComponentModel.DataAnnotations;

namespace NNShop.ViewModels
{
    public class LoginVM
    {
        
            [Display(Name = "Tên đăng nhập")]
            [Required(ErrorMessage = "Chưa nhập tên đăng nhập")]
            public int UserName { get; set; }

            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Chưa nhập mật khẩu")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        
    }
}
