using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Grad
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Naziv { get; set; }

        [Range(0, 1000)]
        public int BrojStanovnika { get; set; }

        [Range(0, 1000)]
        public int BrojNekretnina { get; set; }
    }
}