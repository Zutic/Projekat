using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Kompanija
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        //[JsonIgnore]
        public List<UgovorKompanije> KompanijaUgovoriKompanije { get; set; }
    }
}