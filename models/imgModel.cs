namespace mascotas.Models
{
    public class imgModel
    {
        public string fileName { get; set; }
        public string url { get; set; }
    }

    public class postImgDTO
    {
        public string fileName { get; set; }
        public int idPostPet { get; set; }
        public string url { get; set; }
    }

    public class updatePostImgDTO : imgModel{
        public int? idImage { get; set; }

    }
}