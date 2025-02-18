using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Editorial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEditorial { get; set; }

        public string Nombre { get; set; }

        // Relación con los libros: una editorial puede tener varios libros
        public virtual ICollection<Libro> Libros { get; set; }
    }
}
