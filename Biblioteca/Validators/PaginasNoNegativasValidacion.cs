using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Validators
{
    public class PaginasNoNegativasValidacion : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int paginas && paginas < 0)
            {
                return new ValidationResult("El número de páginas no puede ser negativo");
            }

            return ValidationResult.Success;
        }
    }
}
