using System.ComponentModel.DataAnnotations;

namespace ProniaWebApp.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
