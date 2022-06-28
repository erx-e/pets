using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class PetState
    {
        public PetState()
        {
            PostPets = new HashSet<PostPet>();
        }

        public string IdState { get; set; }
        public string StateName { get; set; }

        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
