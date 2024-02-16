using FluentValidation;
using GryAuthServer.Core.DTOs;

namespace GryAuthServer.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            //             Email Boş olamaz. Boş olur ise göster:       Bu bir Email adresidir.Email @ yoksa göster:        
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is wrong");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
}
