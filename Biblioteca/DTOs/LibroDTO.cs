namespace Biblioteca.DTOs
{
    public class LibroDTO
    {
        public int Isbn { get; set; }
        public string Titulo { get; set; }
        public int Paginas { get; set; }
        public decimal Precio { get; set; }
        public string? FotoPortada { get; set; }
        public bool Descatalogado { get; set; }
        public string NombreAutor { get; set; }
        public string NombreEditorial { get; set; }
    }
}
