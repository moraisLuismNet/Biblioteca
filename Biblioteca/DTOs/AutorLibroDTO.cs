namespace Biblioteca.DTOs
{
    public class AutorLibroDTO
    {
        public int IdAutor { get; set; }
        public string Nombre { get; set; }
        public int TotalLibros { get; set; }
        public decimal PromedioPrecios { get; set; }
        public List<LibroItemDTO> Libros { get; set; }
    }
}
