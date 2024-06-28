using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PostManager.API.DTO;
using PostManager.API.Utils;
using PostManager.Domain.Model;
using PostManager.Service;
using PostManager.Service.Services;
using PostManager.Service.Validators;
using System.Security.Claims;

namespace PostManager.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cadastra uma postagem para o sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "descricao": "Teste"
        ///     }
        /// </remarks>
        /// <returns>Postagem cadastrada ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Postagem cadastrada</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpPost(Name = "Enviar")]
        public IActionResult Add([FromBody] PostDTO postagem)
        {
            try
            {
                postagem.Data = DateTime.Now;
                var currentUser = TokenManager.GetCurrentUser(HttpContext);
                postagem.Usuario = currentUser;
                var result = _service.Add<PostDTO, Post, PostValidator>(postagem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Atualiza uma postagem no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "id":1,
        ///        "descricao": "Teste"
        ///     }
        /// </remarks>
        /// <returns>Postagem atualiza ou uma mensagem de erro caso a atualização não seja realizada</returns>
        /// <response code="200">Postagem atualizada</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpPut(Name = "Editar")]
        public IActionResult Update([FromBody] PostDTO postagem)
        {
            try
            {
                var currentUser = TokenManager.GetCurrentUser(HttpContext);
                postagem.Usuario = currentUser;
                _service.Update(postagem);
                return Ok("Postagem atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Exclui uma postagem no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "id": 1
        ///     }
        /// </remarks>
        /// <returns>Mensagem de exclusão ou mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpDelete(Name = "Excluir")]
        public IActionResult Delete(int id)
        {
            try
            {
                var currentUser = TokenManager.GetCurrentUser(HttpContext);                
                _service.Delete(id, currentUser.Id);
                return Ok("Postagem excluida com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Retorna uma postagem cadastrada no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "id": 1
        ///     }
        /// </remarks>
        /// <returns>Postagem cadastrada ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Postagem cadastrada</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpGet(Name = "Retornar")]
        public IActionResult Get(int id)
        {
            try
            {
                var currentUser = TokenManager.GetCurrentUser(HttpContext);
                var result = _service.GetByUser<PostDTO>(id,currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista de postagens do usuário corrente.
        /// </summary>
        /// <returns>Lista de postagens cadastradas ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Postagens cadastradas pelo usuário corrente</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpGet(Name = "ListarPorUsuario")]
        public IActionResult ListByUser()
        {
            try
            {
                var currentUser = TokenManager.GetCurrentUser(HttpContext);
                var result = _service.ListByUser<PostDTO>(currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lista postagens cadastradas por outros usuários no sistema.
        /// </summary>
        /// <returns>Lista postagens cadastradas por outros usuários ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Postagens cadastradas por outros usuários</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        /// <response code="401">Mensagem de erro caso usuário não esteja autorizado</response>
        [HttpGet(Name = "Notificar")]
        public IActionResult ListLast(string email)
        {
            try
            {
                var result = _service.ListLast<PostDTO>(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
