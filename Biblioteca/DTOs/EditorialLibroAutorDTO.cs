namespace Biblioteca.DTOs
{
    public class EditorialLibroAutorDTO
    {
        public string Nombre { get; set; }
        public List<LibroItemDTO> Libros { get; set; }
        public List<AutorDTO> Autores { get; set; }
    }
}
