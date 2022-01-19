using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class UgovorKompanije
    {
        [Key]
        public int ID { get; set; }

        public Kompanija Prodavac { get; set; }

        //[JsonIgnore]
        public Stanovnik Kupac { get; set; }

        public double Cena { get; set; }

        [JsonIgnore]
        public Nekretnina Objekat { get; set; }

        public int NekretninaFK { get; set; }
    }

}