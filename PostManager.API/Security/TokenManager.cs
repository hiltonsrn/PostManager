
using PostManager.API.DTO;
using System.Security.Claims;

namespace PostManager.API.Utils
{
    public class TokenManager
    {
        internal static UsuarioDTO GetCurrentUser(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            return new UsuarioDTO
            {
                Id = int.Parse(identity.FindFirst("id").Value),
                Email = identity.FindFirst("id").Value
            };
        }
    }
}
