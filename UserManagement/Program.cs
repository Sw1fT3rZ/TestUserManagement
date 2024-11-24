using Microsoft.AspNetCore.Authentication.JwtBearer; // Для работы с JWT-аутентификацией
using Microsoft.EntityFrameworkCore; // Для работы с базой данных через Entity Framework Core
using Microsoft.IdentityModel.Tokens; // Для проверки токенов
using System.Text; // Для работы с текстом 
using UserManagementApp.Data; // Подключаем класс для работы с бд

var builder = WebApplication.CreateBuilder(args);

// Настраиваем подключение к базе данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Подключаем поддержку контроллеров
builder.Services.AddControllers();

// Подключаем Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настраиваем JWT-аутентификацию
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Указываем, что используем JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Проверяем, кто выдал токен 
            ValidateAudience = true, // Проверяем, кто может использовать токен 
            ValidateLifetime = true, // Проверяем срок действия токена
            ValidateIssuerSigningKey = true, // Проверяем подпись токена
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Берем издателя токена из настроек
            ValidAudience = builder.Configuration["Jwt:Audience"], // Берем аудиторию из настроек
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Указываем ключ для проверки подписи
        };
    });

// Подключаем авторизацию
builder.Services.AddAuthorization();

var app = builder.Build();

// Включаем Swagger, если запущено в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Включаем генерацию документации
    app.UseSwaggerUI(); // Включаем интерфейс для тестирования API через Swagger
}

// Подключаем аутентификацию и авторизацию
app.UseAuthentication(); // Аутентификация: проверка токена
app.UseAuthorization(); // Авторизация: проверка прав доступа

// Указываем, что приложение будет использовать контроллеры для обработки запросов
app.MapControllers();

// Запускаем приложение
app.Run(); // Начинаем принимать запросы