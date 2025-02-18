namespace Biblioteca.DTOs
{
    public class EditorialLibroDTO
    {
        public int IdEditorial { get; set; }
        public string Nombre { get; set; }
        public List<LibroItemDTO> Libros { get; set; }
    }
}
