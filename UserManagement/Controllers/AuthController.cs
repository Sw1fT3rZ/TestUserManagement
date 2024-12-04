using Microsoft.AspNetCore.Mvc; // Для работы с API-контроллерами
using Microsoft.IdentityModel.Tokens; // Для работы с токенами
using System.IdentityModel.Tokens.Jwt; // Для генерации JWT-токенов
using System.Security.Claims; // Для создания claims
using System.Text; // Для работы с текстом
using UserManagementApp.Data; // Для работы с базой данных
using UserManagementApp.Models; 

namespace UserManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Контекст базы бд
        private readonly IConfiguration _configuration; // Для получения настроек (ключа, Issuer, Audience)

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context; // Сохраняем контекст бд
            _configuration = configuration; // Сохраняем настройки
        }

        // Метод для входа и получения JWT токена
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Проверяем пользователя в бд
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null) // Если пользователь с таким Email не найден
                return Unauthorized(new { message = "Invalid email or password" });
            var token = GenerateJwtToken(user); // Генерируем токен для найденного юзера
            return Ok(new { token }); // Возвращаем токен клиенту
        }

        // Метод для генерации JWT токена
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Добавляем email в claims
                new Claim("name", user.Name), // Добавляем имя пользователя
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Уникальный идентификатор токена
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Ключ из appsettings.json
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Алгоритм подписи

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Издатель токена
                audience: _configuration["Jwt:Audience"], // Аудитория токена
                claims: claims, // Добавляем claims
                expires: DateTime.Now.AddMinutes(30), // Устанавливаем срок действия токена
                signingCredentials: creds); // Добавляем подпись токена

            return new JwtSecurityTokenHandler().WriteToken(token); // Возвращаем токен как строку
        }
    }

    // Модель для логина
    public class LoginModel
    {
        public string Email { get; set; } // Email 
        public string Password { get; set; } // Пароль 
    }
}