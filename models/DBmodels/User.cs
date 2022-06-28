using System;
using System.Collections.Generic;


namespace mascotas
{
    public partial class User
    {
        public User()
        {
            PostPets = new HashSet<PostPet>();
        }

        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? CellNumber { get; set; }
        public string? FacebookProfile { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
