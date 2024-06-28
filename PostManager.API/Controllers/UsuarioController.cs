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
        /// Efetua o cadastro de usu�rio no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "nome": "Jos�",
        ///        "email": "jose@email.com",
        ///        "senha": "123456",
        ///        "confirmSenha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usu�rio cadastrado ou uma mensagem de erro caso o cadastro n�o seja realizado</returns>
        /// <response code="200">Usu�rio cadastrado com sucesso</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na opera��o</response>
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
        /// Altera o cadastro do usu�rio no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "nome": "Jos�",
        ///        "email": "jose@email.com",
        ///        "senha": "123456",
        ///        "confirmSenha": "123456",
        ///        "novaSenha": "123456",
        ///        "confirmNovaSenha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usu�rio utualizado ou uma mensagem de erro caso o cadastro n�o seja realizado</returns>
        /// <response code="200">Usu�rio atualizado com sucesso</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na opera��o</response>
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
        /// Retorna o usu�rio atrav�s do E-mail e senha cadastrados.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     {
        ///        "email": "jose@email.com",
        ///        "senha": "123456"
        ///     }
        /// </remarks>
        /// <returns>Usu�rio registrado ou uma mensagem de erro caso o cadastro n�o seja realizado</returns>
        /// <response code="200">Usu�rio cadastrado</response>
        /// <response code="400">Mensagem de erro informando o que houve de errado na opera��o</response>
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
