using FluentValidation;
using PostManager.Domain.Model;

namespace PostManager.Service.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("Informe sua mensagem!")
                .NotNull().WithMessage("Informe sua mensagem!");
        }
    }
}
