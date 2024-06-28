using AutoMapper;
using FluentValidation;
using PostManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using PostManager.Infra.Data.Repository;
using PostManager.Service.Validators;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using PostManager.Global;

namespace PostManager.Service.Services
{
    public class UsuarioService : BaseService<Usuario>, IUsuarioService
    {
        private readonly IConfiguration _configuration;
        public UsuarioService(IBaseRepository<Usuario> baseRepository,
                              IMapper mapper,
                              IConfiguration configuration) : base(baseRepository, mapper)
        {
            _configuration = configuration;
        }

        public Usuario Add<TInputModel>(TInputModel inputModel)
            where TInputModel : class
        {
            Usuario entity = _mapper.Map<Usuario>(inputModel);
            Validate(entity, new UsuarioValidator(false));
            entity.Senha = entity.ConfirmSenha = MD5Hash.CalculaHash(entity.Senha);
            var existente = _baseRepository.Select().FirstOrDefault(x => x.Email == entity.Email);
            if (existente != null)
            {
                throw new Exception("E-Mail já cadastrado");
            }
            _baseRepository.Insert(entity);
            return entity;
        }
        public Usuario Update<TInputModel>(TInputModel inputModel)
            where TInputModel : class
        {
            Usuario entity = _mapper.Map<Usuario>(inputModel);
            Validate(entity, new UsuarioValidator(true));
            entity.Senha = MD5Hash.CalculaHash(entity.Senha);
            entity = Login<Usuario>(entity);
            if(entity == null)
            {
                throw new Exception("Usuário não encontrado!");
            }
            entity.Senha = entity.ConfirmSenha = MD5Hash.CalculaHash(_mapper.Map<Usuario>(inputModel).NovaSenha);
            _baseRepository.Update(entity);
            return entity;
        }

        public string GenToken(int id, string email)
        {
            var claims = new[]
            {
                new Claim("id",id.ToString()),
                new Claim("email",email),
                new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString()),
            };
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secretkey"]));
            var creditials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(120);
            var token = new JwtSecurityToken(issuer: _configuration["jwt:issuer"],
                                            audience: _configuration["jwt:audience"],
                                            claims: claims,
                                            expires: expiration,
                                            signingCredentials: creditials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TOutputModel Login<TOutputModel>(Usuario inputModel)
            where TOutputModel : class
        {
            var usuarios = _baseRepository.Select();
            var usuario = usuarios.FirstOrDefault(item=> item.Email == inputModel.Email);
            if (usuario == null)
                throw new Exception("Usuário não encontrado!");
            if(usuario.Senha != inputModel.Senha)
                throw new Exception("Senha incorreta!");
            return _mapper.Map<Usuario, TOutputModel>(usuario);
        }
    }
}
