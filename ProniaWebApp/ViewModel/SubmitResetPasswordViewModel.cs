using System.ComponentModel.DataAnnotations;

namespace ProniaWebApp.ViewModel
{
    public class SubmitResetPasswordViewModel
    {
        [Required , DataType (DataType.Password)]
        public string Password { get; set; }
        [Required , DataType (DataType.Password),Compare(nameof(Password))]

        public string PasswordConfirm { get; set; }
    }
}
