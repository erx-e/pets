using System.ComponentModel.DataAnnotations;

namespace mascotas.Models
{
    public class imgModel
    {
        [Url(ErrorMessage ="must send a valid url")]
        public string url { get; set; }
    }

    public class createImgDTO : imgModel{
    }

    public class postImgDTO
    {
        public int idPostPet { get; set; }
        [Url(ErrorMessage ="must send a valid url")]
        public string url { get; set; }
    }

    public class updatePostImgDTO
    {
        public int? idPostPet { get; set; }

        [Url(ErrorMessage ="must send a valid url")]
        public string? url { get; set; }

        public int? idImage { get; set; }

    }
}