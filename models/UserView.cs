using System.ComponentModel.DataAnnotations;

namespace mascotas.Models
{
    public class UserView
    {
        public int? idUser { get; set; }

        [Required(ErrorMessage = "{0} must be send")]
        [MaxLength(60, ErrorMessage ="{0} must have a max of 60 characters")]
        [RegularExpression(@"^([Aa-zA-ZáéíóúÁÉÍÓÚÑñ]{2,}\s?){2,4}$",
                            ErrorMessage = "must send a valid name using {1}")]
        public string name { get; set; }

        [Required(ErrorMessage = "{0} must be send")]
        [MaxLength(100, ErrorMessage ="{0} must have a max of 100 characters")]
        [EmailAddress(ErrorMessage = "must send be a valid email")]
        public string email { get; set; }

        [RegularExpression(@"^([0-9]{10}\s{1}){1,3}([0-9]{10}\s*)?$",
                            ErrorMessage = "must send cell numbers separated by spaces using the regex {1}")]
        public string? cellNumber { get; set; }
        public string? facebookProfile { get; set; }

    }
    public class CreateUserDTO : UserView
    {
        [Required(ErrorMessage = "Must send {0}")]
        [MinLength(6, ErrorMessage ="Must send at least 6 characters")]
        [RegularExpression(@"^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{6,})\S$",
                ErrorMessage = "Must send a valid password that contains at least one capital letter, one number, no spaces")]
        public string password { get; set; }
    }
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Must send {0}")]
        public int idUser { get; set; }

        [MaxLength(60, ErrorMessage ="{0} must have a max of 60 characters")]
        [RegularExpression(@"^([Aa-zA-ZáéíóúÁÉÍÓÚÑñ]{2,}\s?){2,4}$", ErrorMessage = "must send a valid name using {1}")]
        public string? name { get; set; }

        [MaxLength(100, ErrorMessage ="{0} must have a max of 100 characters")]
        [EmailAddress(ErrorMessage = "must send be a valid email")]
        public string? email { get; set; }

        [MaxLength(45, ErrorMessage = "The {0} can not have more than {1} characters")]
        [RegularExpression(@"^([0-9]{10}\s{1}){1,3}([0-9]{10}\s*)?$", 
        ErrorMessage = "must send cell numbers separated by spaces using the regex {1}")]
        public string? cellNumber { get; set; }

        public string? facebookProfile { get; set; }

        [MinLength(6, ErrorMessage = "{0} must have 6 characters")]
        [RegularExpression(@"^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{6,})\S$",
        ErrorMessage = "Must send a valid password that contains at least one capital letter, one number, no spaces")]
        public string? password { get; set; }
        public string? oldPassword { get; set; }
    }

    public class UserResponse
    {
        public UserView user { get; set; }
        public string token { get; set; }
    }

    public class AuthUserRequest
    {
        [Required(ErrorMessage = "{0} must be send")]
        [EmailAddress(ErrorMessage = "must send be a valid email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Must send {0}")]
        public string password { get; set; }
    }
}