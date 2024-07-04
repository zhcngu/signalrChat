using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;

namespace SignalRChat.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : Controller
    {
        private readonly IHubContext<StronglyTypedChatHub,IChatClient> _hubContext;
        public ValueController(IHubContext<StronglyTypedChatHub, IChatClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet(nameof(All))]
        public async Task<IActionResult> All()
        {
            return Ok(ChatClients.GetAll());
        }

        [HttpGet(nameof(GetValue))]
        public async Task<IActionResult> GetValue([FromQuery] string id)
        {
            var client = ChatClients.GetClient(item => item.Id == id);
            return Ok($"{DateTime.Now},id={id}, value={client.Stars}");
        }
        [HttpGet(nameof(Update))]
        public async Task<IActionResult> Update([FromQuery] string userName, [FromQuery] int value)
        {
            var client = ChatClients.GetClient(item => item.Name == userName);
            client.Stars = value;
          
            //await _hubContext.Clients.Group(client.Group).SendAsync("ReceiveMessage", userName, "Group");
            await _hubContext.Clients.AllExcept(client.Id).ReceiveMessage( userName,$"pay attention {userName} hava {value} stars");
            await _hubContext.Clients.Client(client.Id).Notification(userName, $"you have {value} stars");
            return Ok($"{DateTime.Now},id={userName}, value={value}");
        }


        [HttpGet(nameof(test))]
        public async Task<IActionResult> test()
        {
            return Ok($"{DateTime.Now}");
        }

    }
}
