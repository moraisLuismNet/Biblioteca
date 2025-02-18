using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLibro { get; set; }

        public string Titulo { get; set; }

        public int Paginas { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Precio { get; set; }

        public string? FotoPortada { get; set; }

        public bool Descatalogado { get; set; }

        // Claves foráneas para la relación con Autor y Editorial

        [ForeignKey("AutorId")]
        // Clave foránea para la relación con Autor
        public int AutorId { get; set; }
        public virtual Autor Autor { get; set; }

        [ForeignKey("EditorialId")]
        // Clave foránea para la relación con Editorial
        public int EditorialId { get; set; }
        public virtual Editorial Editorial { get; set; }
    }
}
