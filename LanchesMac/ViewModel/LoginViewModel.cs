using System.ComponentModel.DataAnnotations;

namespace LanchesMac.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Inform o nome")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
