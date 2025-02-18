using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Operacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOperacion { get; set; }

        public DateTime FechaAccion { get; set; }

        public string Operation { get; set; }

        public string Controller { get; set; }

        public string Ip { get; set; }
    }
}