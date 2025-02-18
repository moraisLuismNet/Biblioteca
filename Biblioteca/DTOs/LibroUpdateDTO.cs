﻿using Biblioteca.Validators;

namespace Biblioteca.DTOs
{
    public class LibroUpdateDTO
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }

        [PaginasNoNegativasValidacion]
        public int Paginas { get; set; }

        public decimal Precio { get; set; }

        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile? Foto { get; set; }

        public bool Descatalogado { get; set; }
        public int AutorId { get; set; }
        public int EditorialId { get; set; }
    }
}
