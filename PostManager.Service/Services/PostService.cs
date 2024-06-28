using AutoMapper;
using FluentValidation;
using PostManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using PostManager.Infra.Data.Repository;
using PostManager.Service.Validators;
using System.Linq;
namespace PostManager.Service.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        public PostService(IBaseRepository<Post> baseRepository, IBaseRepository<Usuario> usuarioRepository,
        IMapper mapper, IPostRepository repository) : base(baseRepository, mapper)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public IEnumerable<TOutputModel> ListByUser<TOutputModel>(int idUser)
        {
            var ret = _baseRepository.Select().Where(x => x.UsuarioId == idUser).ToList();
            return ret.Select(s => _mapper.Map<TOutputModel>(s));
        }
        public IEnumerable<TOutputModel> ListLast<TOutputModel>(string email)
        {
            var usuarios = _usuarioRepository.Select();
            var usuario = usuarios.FirstOrDefault(item => item.Email == email);
            if (usuario == null)
            {
                throw new Exception("E-mail não cadastrado!");
            }
            var result = _repository.GetLast(email);
            _repository.AddNotifications(result, usuario.Id);
            return result.Select(s => _mapper.Map<TOutputModel>(s)); ;
        }
        public void Delete(int Id, int UserId)
        {
            _repository.DeleteNotification(Id);
            _repository.Delete(Id, UserId);
        }

        public void Update<TInputModel>(TInputModel post)
        {
            Post postInput = _mapper.Map<Post>(post);
            _repository.DeleteNotification(postInput.Id);
            var oldPost = _repository.GetByUser(postInput.Id, postInput.UsuarioId);
            if (oldPost == null)
            {
                throw new Exception("Postagem não encontrada!");
            }
            oldPost.Descricao = postInput.Descricao;
            _repository.Update(oldPost);
        }

        public TOutputModel GetByUser<TOutputModel>(int id, int idUser)
        {
            var model = _repository.GetByUser(id, idUser);
            TOutputModel result = _mapper.Map<TOutputModel>(model);
            return result;
        }
    }
}
