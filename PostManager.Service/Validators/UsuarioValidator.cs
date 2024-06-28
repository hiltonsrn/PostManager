using FluentValidation;
using Microsoft.Identity.Client;
using PostManager.Domain.Model;

namespace PostManager.Service.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator(bool edit)
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Informe o seu nome!")
                .NotNull().WithMessage("Informe o seu nome!");
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Informe o seu E-Mail!")
                .NotNull().WithMessage("Informe o seu E-Mail!");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("Informe sua senha!")
                .NotNull().WithMessage("Informe sua senha!");
            RuleFor(c => c.ConfirmSenha)
               .NotEmpty().WithMessage("Informe a confirmação da sua senha!")
               .NotNull().WithMessage("Informe a confirmação da sua senha!");
            RuleFor(c => c.Senha)
                .MinimumLength(6).WithMessage("A senha deve conter pelo menos 6 caracteres!");
            

            RuleFor(c => c.Email)
               .EmailAddress().WithMessage("E-mail inválido!");
            RuleFor(c => c.Senha)
                .Equal(p => p.ConfirmSenha)
                .WithMessage("Confirmação diferente da senha!");

            RuleFor(c => c.NovaSenha)
                .NotEmpty().WithMessage("Informe sua senha!").When(p => edit)
                .NotNull().WithMessage("Informe sua senha!").When(p => edit);
            RuleFor(c => c.ConfirmNovaSenha)
               .NotEmpty().WithMessage("Informe a confirmação da sua senha!").When(p => edit)
               .NotNull().WithMessage("Informe a confirmação da sua senha!").When(p => edit);
            RuleFor(c => c.NovaSenha)
                .MinimumLength(6).MinimumLength(6).WithMessage("A nova senha deve conter pelo menos 6 caracteres!").When(p => edit);            

            RuleFor(c => c.NovaSenha)
                .Equal(p => p.ConfirmNovaSenha)
                .When(p=> edit)
                .WithMessage("Confirmação da nova senha diferente da nova senha!");
        }
    }
}
