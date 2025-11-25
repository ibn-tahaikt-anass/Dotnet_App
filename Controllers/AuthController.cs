using MES.Data;
using MES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace MES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ProductsContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ProductsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Email already exists");

            user.PasswordHash = HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDto login)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
        //    if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
        //        return Unauthorized("Invalid email or password");

        //    var token = GenerateJwtToken(user);
        //    return Ok(new { token });
        //}

        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            var token = GenerateJwtToken(user);

            // ✅ Build the response object safely
            var response = new
            {
                id = user.Id,
                userName = user.UserName ?? user.Email.Split('@')[0],
                email = user.Email,
                role = user.Role ?? "Employee",
                token = token,
                refreshToken = token,
                ability = new[]
                {
            new { action = "manage", subject = "all" }
        }
            };

            return Ok(response);
        }


        // 🔐 Helper: Generate JWT
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ) // ✅ Added Role claim
        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // 🔐 Helper: Password Hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
