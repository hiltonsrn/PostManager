using FluentValidation;
using PostManager.Domain.Model;
using System.Collections.Generic;

namespace PostManager.Service
{
    public interface IUsuarioService : IBaseService<Usuario>
    {
        TOutputModel Login<TOutputModel>(Usuario inputModel)
            where TOutputModel : class;
        Usuario Add<TInputModel>(TInputModel inputModel)
           where TInputModel : class;
        Usuario Update<TInputModel>(TInputModel inputModel)
             where TInputModel : class;    
         string GenToken(int id, string email); 
    }
}
