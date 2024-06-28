using Microsoft.AspNetCore.SignalR;
using PostManager.API.DTO;
using PostManager.Service;

namespace PostManager.API.HUBs
{
    public class PostHub : Hub
    {
        private readonly IPostService _service;
        public PostHub(IPostService service)
        {
            _service = service;
        }
        public async IAsyncEnumerable<string> Notificar(CancellationToken cancellation)
        {
            while (true)
            {
                string email = Context.GetHttpContext().GetRouteValue("email").ToString();
                var result = _service.ListLast<PostDTO>(email);
                var str = System.Text.Json.JsonSerializer.Serialize(result.ToList());
                yield return str;
               Task.Delay(1000, cancellation);
            }
        }
    }
}
