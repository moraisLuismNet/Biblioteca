namespace Biblioteca.DTOs
{
    public class EditorialLibroAutorDTO
    {
        public string NombreEditorial { get; set; }
        public List<LibroItemDTO> Libros { get; set; }
        public List<AutorDTO> Autores { get; set; }
    }
}
