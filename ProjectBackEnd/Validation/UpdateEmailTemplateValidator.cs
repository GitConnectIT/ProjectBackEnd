using FluentValidation;
using Shared.DTO;

namespace ProjectBackEnd.Validation
{
    public class UpdateEmailTemplateDTOValidator : AbstractValidator<EmailTemplateDTO>
    {
        public UpdateEmailTemplateDTOValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(1, 128).WithMessage("Name cannot be null and with size between 1 to 128");
            RuleFor(x => x.Code).NotNull().NotEmpty().Length(1, 50).WithMessage("Code cannot be null and with size between 1 to 50");
            RuleFor(x => x.Subject).NotNull().NotEmpty().Length(1, 50).WithMessage("Subject cannot be null and with size between 1 to 50");
        }
    }
}
