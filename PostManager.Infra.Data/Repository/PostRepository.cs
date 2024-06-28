using Microsoft.EntityFrameworkCore;
using PostManager.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace PostManager.Infra.Data.Repository
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(LocalDBMSSQLLocalDBContext mySqlContext): base(mySqlContext) {
            
        }
        public void Delete(int Id, int userId) { 
            var postagem = _mySqlContext.Posts.FirstOrDefault(p => p.Id == Id && p.UsuarioId == userId);
            if (postagem == null)
            {
                throw new Exception("Postagem não encontrada!");
            }
            _mySqlContext.Posts.Remove(postagem);
            _mySqlContext.SaveChanges();
        }
        public Post GetByUser(int Id, int UsuarioId)
        {
            return _mySqlContext.Posts.FirstOrDefault(item=> item.Id == Id && item.UsuarioId == UsuarioId);
        }
        public IList<Post> GetLast(string email)
        {
            return (from p in _mySqlContext.Posts
                          join u in _mySqlContext.Usuarios on p.UsuarioId equals u.Id
                          where u.Email != email
                          where (from n in _mySqlContext.Notificacaos
                                 join u in _mySqlContext.Usuarios
                                 on n.UsuarioId equals u.Id
                                 where u.Email == email && n.PostId == p.Id
                                 select n).Count() == 0
                          select p).OrderByDescending(item => item.Data).ToList();
        }
        public void AddNotifications(IEnumerable<Post> posts, int idUser)
        {
            foreach (var post in posts)
            {
                var notificacao = new Notificacao
                {
                    PostId = post.Id,
                    UsuarioId = idUser
                };
                _mySqlContext.Notificacaos.Add(notificacao);
            }
            _mySqlContext.SaveChanges();
        }

        public void DeleteNotification(int PostId)
        {
            var notificacoes = _mySqlContext.Notificacaos.Where(item => item.PostId == PostId).ToList();
            _mySqlContext.RemoveRange(notificacoes);
            _mySqlContext.SaveChanges();
        }
    }
}
