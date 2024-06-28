using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PostManager.API.DTO;
using PostManager.API.Utils;
using PostManager.Domain.Model;
using PostManager.Global;
using PostManager.Service;
using PostManager.Service.Services;
using PostManager.Service.Validators;

namespace PostManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        /// <summary>
        /// Efetua o cadastro de usuário no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "nome": "José",
        ///        "email": "jose@email.com",
        ///        "senha": "123456",
        ///        "confirmSenha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usuário cadastrado ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Usuário cadastrado com sucesso</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        [HttpPost(Name = "Registrar")]
        public IActionResult Add([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var result = _service.Add<UsuarioDTO>(usuario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Altera o cadastro do usuário no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "nome": "José",
        ///        "email": "jose@email.com",
        ///        "senha": "123456",
        ///        "confirmSenha": "123456",
        ///        "novaSenha": "123456",
        ///        "confirmNovaSenha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usuário utualizado ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        [HttpPut(Name = "Alterar")]
        public IActionResult Update([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var result = _service.Update<UsuarioDTO>(usuario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Retorna o usuário através do E-mail e senha cadastrados.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "email": "jose@email.com",
        ///        "senha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usuário registrado ou uma mensagem de erro caso o cadastro não seja realizado</returns>
        /// <response code="200">Usuário cadastrado</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na operação</response>
        [HttpGet(Name = "Login")]
        public IActionResult Get(string email, string senha)
        {
            try
            {
                var usuario = new UsuarioDTO
                {
                    Email = email,
                    Senha = MD5Hash.CalculaHash(senha)
                };
                var result = _service.Login<UsuarioDTO>(_mapper.Map<Usuario>(usuario));
                var token = _service.GenToken(result.Id, result.Email);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
