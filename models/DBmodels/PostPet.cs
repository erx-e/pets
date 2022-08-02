using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class PostPet
    {
        public PostPet()
        {
            PostImages = new HashSet<PostImage>();
        }

        public int IdPostPet { get; set; }
        public int IdUser { get; set; }
        public string PetName { get; set; }
        public string IdState { get; set; }
        public int IdPetSpecie { get; set; }
        public int IdPetBreed { get; set; }
        public int IdProvincia { get; set; }
        public int IdCanton { get; set; }
        public int? IdSector { get; set; }
        public string Description { get; set; }
        public decimal? Reward { get; set; }
        public DateTime LastTimeSeen { get; set; }
        public string LinkMapSeen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Canton IdCantonNavigation { get; set; }
        public virtual PetBreed IdPetBreedNavigation { get; set; }
        public virtual PetSpecie IdPetSpecieNavigation { get; set; }
        public virtual Provincium IdProvinciaNavigation { get; set; }
        public virtual Sector IdSectorNavigation { get; set; }
        public virtual PetState IdStateNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
