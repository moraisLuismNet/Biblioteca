using Biblioteca.DTOs;
using FluentValidation;

namespace Biblioteca.Validators
{
    public class EditorialInsertValidator : AbstractValidator<EditorialInsertDTO>
    {
        public EditorialInsertValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre is required");
            RuleFor(x => x.Nombre).Length(2, 20).WithMessage("Nombre must be between 2 and 20 characters");
        }
    }
}
