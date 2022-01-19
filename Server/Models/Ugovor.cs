using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Ugovor
    {
        [Key]
        public int ID { get; set; }

        public Stanovnik Kupac { get; set; }

        public Stanovnik Prodavac { get; set; }
        
        public int Procenat { get; set; }

        public double Cena { get; set; }

        public Nekretnina Objekat { get; set; }
    }
}