using PostManager.Domain.Model;
using System.Collections.Generic;

namespace PostManager.Infra.Data.Repository
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        IList<Post> GetLast(string email);
        void AddNotifications(IEnumerable<Post> posts, int idUser);
        void Delete(int Id, int userId);
        Post GetByUser(int Id, int UsuarioId);
        void DeleteNotification(int PostId);
    }
}
