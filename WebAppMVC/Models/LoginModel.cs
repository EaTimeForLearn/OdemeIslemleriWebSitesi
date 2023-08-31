using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebAppMVC.Models
{
    public class LoginModel
    {
            [Required(ErrorMessage="Kullanıcı adı boş bırakılamaz!")]           
            [Display(Name = "Username")]
            public string UserName  {   get;set;  }

        [Required(ErrorMessage = "Şifre boş bırakılamaz!")]
        [DataType(DataType.Password)]
            public string Password { get;set;}
            public bool RememberLogin {get;set;}
        
    }
}
