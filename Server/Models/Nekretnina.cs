using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public enum TIP
    {
        KUCA,
        STAN,
        LOKAL,
        ZGRADA
    }
    [Table("Nekretnina")]
    public class Nekretnina
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public TIP Tip { get; set; }

        public Grad Lokacija { get; set; }

        public Kompanija Graditelj { get; set; }

        public double PocetnaCena { get; set; }

        //[JsonIgnore]
        public List<Ugovor> NekretninaUgovori { get; set; }

        //[JsonIgnore]
        public UgovorKompanije NekretninaUgovorKompanije { get; set; }

        [Required]
        public string Slika { get; set; }
    }
}