using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class PetBreed
    {
        public PetBreed()
        {
            PostPets = new HashSet<PostPet>();
        }

        public int IdPetBreed { get; set; }
        public string BreedName { get; set; }
        public int IdSpecie { get; set; }

        public virtual PetSpecie IdSpecieNavigation { get; set; }
        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
