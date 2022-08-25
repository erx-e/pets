using System.ComponentModel.DataAnnotations;

namespace mascotas.Models
{
    public class UserView
    {
        public int? idUser {get; set;}
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string? cellNumber { get; set; }
        public string? facebookProfile { get; set; }

    }
    public class CreateUserDTO : UserView
    {
        [Required]
        public string password { get; set; }
    }
    public class UpdateUserDTO
    {
        [Required]
        public int idUser { get; set; }
        public string? name { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        public string? cellNumber { get; set; }
        public string? facebookProfile { get; set; }
        public string? password { get; set; }
        public string? oldPassword {get; set;}
    }

    public class UserResponse
    {
        public UserView user { get; set; }
        public string token { get; set; }
    }

    public class AuthUserRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}