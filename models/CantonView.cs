namespace mascotas.Models
{
    public class CantonView
    {
        public int id { get; set; }
        public string name { get; set; }
        public ProvinciaView provincia { get; set; }
    }
}