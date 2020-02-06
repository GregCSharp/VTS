using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VTS.API.Controllers.Dtos;
using VTS.API.Data;

namespace VTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (await _repo.UserExists(userDto.Name))
                return BadRequest("Username already exists");

            var userToCreate = new User{Name = userDto.Name};
            var createdUser = await _repo.Register(userToCreate);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var userFromRepo = await _repo.Login(userDto.Name);
            if (userFromRepo == null)
                return Unauthorized();

            //builds claims for token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Name)
            };

            //builds a security to make sure the token is a valid one created from the server
            //using a secret key store in the app settings file
            //the key is hashed to not be decoded on the client side
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //creating a token, starting with the token content
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            //to fill a token with the content of the token we must a token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //finally we send token back to the client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}