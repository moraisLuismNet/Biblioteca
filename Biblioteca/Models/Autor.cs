using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Autor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAutor { get; set; }

        public string Nombre { get; set; }

        // Relación con los libros: un autor puede tener varios libros
        public virtual ICollection<Libro> Libros { get; set; }
    }
}
