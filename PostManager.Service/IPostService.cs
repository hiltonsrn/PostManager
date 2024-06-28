using FluentValidation;
using PostManager.Domain.Model;
using System.Collections.Generic;

namespace PostManager.Service
{
    public interface IPostService : IBaseService<Post>
    {
        TOutputModel GetByUser<TOutputModel>(int id, int idUser);
        IEnumerable<TOutputModel> ListByUser<TOutputModel>(int idUser);
        IEnumerable<TOutputModel> ListLast<TOutputModel>(string email);
        void Delete(int Id, int UserId);
        void Update<TInputModel>(TInputModel post);
    }
}
