using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalRChat.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login([FromQuery]string user)
        {
            if (user != null)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user),
                   // new  Claim("design","chat room")
                };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("copyrightasp.netcoresignalrchatroom"));

                var algorithm = SecurityAlgorithms.HmacSha256;

                var signingCredentials = new SigningCredentials(secretKey, algorithm);

                var jwtSecurityToken = new JwtSecurityToken(
                "signalr chat",     //Issuer
                "signalr chat",   //Audience
                claims,                          //Claims,
                 notBefore:null,//notBefore
                expires: DateTime.Now.AddMinutes(1),    //expires
               signingCredentials:   signingCredentials               //Credentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok(token);
            }
           return BadRequest();
        }
    }
}
