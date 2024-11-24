using Microsoft.AspNetCore.Authentication.JwtBearer; // ��� ������ � JWT-���������������
using Microsoft.EntityFrameworkCore; // ��� ������ � ����� ������ ����� Entity Framework Core
using Microsoft.IdentityModel.Tokens; // ��� �������� �������
using System.Text; // ��� ������ � ������� 
using UserManagementApp.Data; // ���������� ����� ��� ������ � ��

var builder = WebApplication.CreateBuilder(args);

// ����������� ����������� � ���� ������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� ��������� ������������
builder.Services.AddControllers();

// ���������� Swagger ��� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����������� JWT-��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // ���������, ��� ���������� JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // ���������, ��� ����� ����� 
            ValidateAudience = true, // ���������, ��� ����� ������������ ����� 
            ValidateLifetime = true, // ��������� ���� �������� ������
            ValidateIssuerSigningKey = true, // ��������� ������� ������
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // ����� �������� ������ �� ��������
            ValidAudience = builder.Configuration["Jwt:Audience"], // ����� ��������� �� ��������
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // ��������� ���� ��� �������� �������
        };
    });

// ���������� �����������
builder.Services.AddAuthorization();

var app = builder.Build();

// �������� Swagger, ���� �������� � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // �������� ��������� ������������
    app.UseSwaggerUI(); // �������� ��������� ��� ������������ API ����� Swagger
}

// ���������� �������������� � �����������
app.UseAuthentication(); // ��������������: �������� ������
app.UseAuthorization(); // �����������: �������� ���� �������

// ���������, ��� ���������� ����� ������������ ����������� ��� ��������� ��������
app.MapControllers();

// ��������� ����������
app.Run(); // �������� ��������� �������