using Biblioteca.DTOs;
using FluentValidation;

namespace Biblioteca.Validators
{
    public class AutorInsertValidator : AbstractValidator<AutorInsertDTO>
    {
        public AutorInsertValidator()
        {
            RuleFor(x => x.NombreAutor).NotEmpty().WithMessage("Nombre is required");
            RuleFor(x => x.NombreAutor).Length(2, 20).WithMessage("Nombre must be between 2 and 20 characters");
        }
    }
}