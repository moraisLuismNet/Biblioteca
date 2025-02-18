using Biblioteca.DTOs;
using FluentValidation;

namespace Biblioteca.Validators
{
    public class EditorialUpdateValidator : AbstractValidator<EditorialUpdateDTO>
    {
        public EditorialUpdateValidator()
        {
            RuleFor(x => x.IdEditorial).NotNull().WithMessage("IdEditorial is required");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre is required");
            RuleFor(x => x.Nombre).Length(2, 20).WithMessage("Nombre must be between 2 and 20 characters");
        }
    }
}