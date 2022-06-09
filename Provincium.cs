using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class Provincium
    {
        public Provincium()
        {
            Cantons = new HashSet<Canton>();
            PostPets = new HashSet<PostPet>();
        }

        public int IdProvincia { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Canton> Cantons { get; set; }
        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
