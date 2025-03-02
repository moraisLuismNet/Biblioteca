namespace Biblioteca.DTOs
{
    public class EditorialLibroDTO
    {
        public int IdEditorial { get; set; }
        public string NombreEditorial { get; set; }
        public int TotalLibros { get; set; }

        public List<LibroItemDTO> Libros { get; set; }
        public decimal PromedioPrecios { get; set; }
        
    }
}
