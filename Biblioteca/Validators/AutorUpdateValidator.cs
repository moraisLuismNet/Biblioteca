using Biblioteca.DTOs;
using FluentValidation;

namespace Biblioteca.Validators
{
    public class AutorUpdateValidator : AbstractValidator<AutorUpdateDTO>
    {
        public AutorUpdateValidator()
        {
            RuleFor(x => x.IdAutor).NotNull().WithMessage("IdAutor is required");
            RuleFor(x => x.NombreAutor).NotEmpty().WithMessage("Nombre is required");
            RuleFor(x => x.NombreAutor).Length(2, 20).WithMessage("Nombre must be between 2 and 20 characters");
        }
    }
}