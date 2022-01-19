using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Stanovnik
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Ime { get; set; }

        [MaxLength(50)]
        public string Prezime { get; set; }

        [StringLength(13)]
        public string JMBG { get; set; }

        [Range(0, 10000000)]
        public double Novac { get; set; }

        public Grad GradStanovanja { get; set; }

        [JsonIgnore]
        public List<UgovorKompanije> StanovnikUgovoriKompanije { get; set; }
    }
}