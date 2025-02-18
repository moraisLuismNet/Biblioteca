namespace Biblioteca.DTOs
{
    public class EditorialLibroAutor
    {
        public string Nombre { get; set; }
        public List<LibroItemDTO> Libros { get; set; }
        public List<AutorDTO> Autores { get; set; }
    }
}
