using System;
using System.Collections.Generic;

#nullable disable

namespace mascotas
{
    public partial class PostImage
    {
        public int IdImage { get; set; }
        public Guid IdImage2 { get; set; }
        public int IdPostPet { get; set; }
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }
        public string Extension { get; set; }

        public virtual PostPet IdPostPetNavigation { get; set; }
    }
}
