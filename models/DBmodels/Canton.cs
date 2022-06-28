using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class Canton
    {
        public Canton()
        {
            PostPets = new HashSet<PostPet>();
            Sectors = new HashSet<Sector>();
        }

        public int IdCanton { get; set; }
        public string Name { get; set; }
        public int IdProvincia { get; set; }

        public virtual Provincium IdProvinciaNavigation { get; set; }
        public virtual ICollection<PostPet> PostPets { get; set; }
        public virtual ICollection<Sector> Sectors { get; set; }
    }
}
