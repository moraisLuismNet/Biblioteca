﻿using Biblioteca.DTOs;
using FluentValidation;

namespace Biblioteca.Validators
{
    public class LibroUpdateValidator : AbstractValidator<LibroUpdateDTO>
    {
        public LibroUpdateValidator()
        {
            RuleFor(x => x.IdLibro).NotEmpty().WithMessage("IdLibro is required");
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Titulo).Length(2, 20).WithMessage("Title must be between 2 and 20 characters");
            RuleFor(x => x.Precio).NotEmpty().WithMessage("Precio is required");
            RuleFor(x => x.Precio).GreaterThan(0).WithMessage("Precio must be greater than 0");
            RuleFor(x => x.AutorId).NotEmpty().WithMessage("AutorId is required");
            RuleFor(x => x.EditorialId).NotEmpty().WithMessage("EditorialId is required");
        }
    }
}