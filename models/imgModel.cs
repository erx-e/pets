namespace mascotas.Models
{
    public class imgModel
    {
        public string url { get; set; }
    }

    public class postImgDTO
    {
        public int idPostPet { get; set; }
        public string url { get; set; }
    }

    public class updatePostImgDTO
    {
        public int? idPostPet { get; set; }

        public string? url { get; set; }

        public int? idImage { get; set; }

    }
}