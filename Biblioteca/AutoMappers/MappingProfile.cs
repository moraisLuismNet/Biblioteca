using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Autor, AutorDTO>().ReverseMap();
            CreateMap<Autor, AutorInsertDTO>().ReverseMap();
            CreateMap<Autor, AutorLibroDTO>().ReverseMap();
            CreateMap<Autor, AutorUpdateDTO>().ReverseMap();
            CreateMap<Editorial, EditorialDTO>().ReverseMap();
            CreateMap<Editorial, EditorialInsertDTO>().ReverseMap();
            CreateMap<Editorial, EditorialLibroAutorDTO>().ReverseMap();
            CreateMap<Editorial, EditorialLibroDTO>().ReverseMap();
            CreateMap<Editorial, EditorialUpdateDTO>().ReverseMap();
            CreateMap<Libro, LibroDTO>().ReverseMap();
            CreateMap<Libro, LibroInsertDTO>().ReverseMap();
            CreateMap<Libro, LibroItemDTO>().ReverseMap();
            CreateMap<Libro, LibroUpdateDTO>().ReverseMap();
        }
    }
}