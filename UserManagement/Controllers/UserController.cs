using Microsoft.AspNetCore.Mvc; // Подключаем нужные инструменты для работы с веб-приложением
using Microsoft.EntityFrameworkCore; // Это нужно, чтобы работать с бд
using UserManagementApp.Data; // Это подключение файла с настройками бд
using UserManagementApp.Models; // Єто подключение файла с описанием модели "User"

namespace UserManagementApp.Controllers // Здесь задаем место, где находится этот код в проекте
{
    [Route("api/[controller]")] // Говорим, что этот контроллер будет доступен по адресу
    [ApiController] // Указіваем, что это контроллер для работы с API
    public class UsersController : ControllerBase // Создаем контроллер, который управляет пользователями
    {
        private readonly ApplicationDbContext _context; // Здесь мы будем хранить связь с бд

        public UsersController(ApplicationDbContext context) // Це конструктор (метод, который запускается при создании объекта этого класса)
        {
            _context = context; // Сохраняем бд, чтобы потом с ней работать
        }

        [HttpGet] // Говорим, что этот метод отвечает за запросы GET
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() // Цей метод возвращает список всех пользователей
        {
            return await _context.Users.ToListAsync(); // Достаем всех пользователей из бд
        }

        [HttpGet("{id}")] // Говорим, что этот метод отвечает за GET-запросы с указанием ID
        public async Task<ActionResult<User>> GetUser(int id) // Этот метод возвращает одного пользователя по его ID
        {
            var user = await _context.Users.FindAsync(id); // Ищем пользователя с указанным ID
            if (user == null) //Если пользователя нет...
            {
                return NotFound(); // ...то возвращаем сообщение, что его не нашли
            }
            return user; // Если нашли, возвращаем данные пользователя
        }

        [HttpPost] // Этот метод срабатывает на POST-запрос (создание нового пользователя)
        public async Task<ActionResult<User>> CreateUser(User user) // Метод создает нового пользователя
        {
            _context.Users.Add(user); // Добавляем нового пользователя в базу
            await _context.SaveChangesAsync(); // Сохраняем изменения в бд

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user); // Отправляем ответ, что пользователь создан, с его данными
        }

        [HttpPut("{id}")] // Этот метод отвечает за обноаление данных пользователя по его ID
        public async Task<IActionResult> UpdateUser(int id, User user) // Метод для изменения данных пользователя
        {
            if (id != user.Id) // Если ID из адреса и ID из данных не совпадают...
            {
                return BadRequest(); // ...возвращаем помилку
            }

            _context.Entry(user).State = EntityState.Modified; // Говорим бд, что данные пользователя изменились
            try
            {
                await _context.SaveChangesAsync(); // Пробуем сохранить изменения
            }
            catch (DbUpdateConcurrencyException) // Если что-то пошло не так...
            {
                if (!UserExists(id)) //...проверяем, существует ли пользователь
                {
                    return NotFound(); // Если пользователя нет, говорим об этом
                }
                else
                {
                    throw; // Если проблема другая, выбрасываем помилку
                }
            }

            return NoContent(); // Если всё прошло успешно, ничего не возвращаем
        }

        [HttpDelete("{id}")] // Этот метод удаляет пользователя по его ID
        public async Task<IActionResult> DeleteUser(int id) // Метод для удаления пользователя
        {
            var user = await _context.Users.FindAsync(id); // Ищем пользователя в базе
            if (user == null) // Если не нашли...
            {
                return NotFound(); // ...говорим, что его нет
            }

            _context.Users.Remove(user); // Удаляем пользователя из базы
            await _context.SaveChangesAsync(); // Сохраняем изменения

            return NoContent(); // Возвращаем, что всё прошло успешно
        }

        private bool UserExists(int id) // Проверяем, есть ли пользователь в базе
        {
            return _context.Users.Any(e => e.Id == id); // Возвращаем "да", если нашли пользователя
        }
    }
}
