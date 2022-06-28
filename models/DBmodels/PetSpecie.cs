using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class PetSpecie
    {
        public PetSpecie()
        {
            PetBreeds = new HashSet<PetBreed>();
            PostPets = new HashSet<PostPet>();
        }

        public int IdPetSpecie { get; set; }
        public string SpecieName { get; set; }

        public virtual ICollection<PetBreed> PetBreeds { get; set; }
        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
