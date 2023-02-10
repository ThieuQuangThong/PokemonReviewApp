using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticate _authenticaRepository;

        public LoginController(IConfiguration config, IAuthenticate authenticaRepository)
        {
            _config = config;
            _authenticaRepository = authenticaRepository;
        }
        /// <summary>
        /// Login.
        /// </summary>
        /// <returns>Login</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await _authenticaRepository.Authenticate(userLogin);

            if (user != null)
            {
                var token = _authenticaRepository.Generate(user);
                Console.WriteLine(token);
                return Ok(token.Result);
            }
            return NotFound("User not found");
        }


    }
}
