namespace Biblioteca.DTOs
{
    public class LibroDTO
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int Paginas { get; set; }
        public decimal Precio { get; set; }
        public string? FotoPortada { get; set; }
        public bool Descatalogado { get; set; }
        public int AutorId { get; set; }
        public int EditorialId { get; set; }
    }
}
