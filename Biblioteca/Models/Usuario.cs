using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        public string? Nombre { get; set; }
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Role { get; set; }

        public byte[]? Salt { get; set; }
    }

}

