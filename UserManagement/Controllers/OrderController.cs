using Microsoft.AspNetCore.Mvc; // Подключаем инструменты для работы с веб-приложением
using Microsoft.EntityFrameworkCore; // Это нужно, чтобы работать с базой данных
using UserManagementApp.Data; // Подключаем файл с настройками базы данных
using UserManagementApp.Models; // Подключаем файл с описанием модели "Order"

namespace UserManagementApp.Controllers // Указываем место, где находится этот код в проекте
{
    [Route("api/[controller]")] // Указываем, что этот контроллер будет доступен по адресу
    [ApiController] // Указываем, что это API-контроллер
    public class OrdersController : ControllerBase // Создаем контроллер, который управляет заказами
    {
        private readonly ApplicationDbContext _context; // Здесь мы будем хранить связь с базой данных

        public OrdersController(ApplicationDbContext context) // Это конструктор 
        {
            _context = context; // Сохраняем базу данных, чтобы потом с ней работать
        }

        [HttpGet] // Говорим, что этот метод отвечает за запросы GET
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() // Этот метод возвращает список всех заказов
        {
            return await _context.Orders.Include(o => o.User).ToListAsync(); // Достаем все заказы из базы вместе с данными пользователей
        }

        [HttpGet("{id}")] // Говорим, что этот метод отвечает за GET-запросы с айди
        public async Task<ActionResult<Order>> GetOrder(int id) // Этот метод возвращает один заказ по его ID
        {
            var order = await _context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id); // Ищем заказ с указанным адйишки в базе
            if (order == null) // Если заказ не найден...
            {
                return NotFound(); // ...то возвращаем сообщение, что его не нашли
            }
            return order; // Если нашли, возвращаем данные заказа
        }

        [HttpPost] // Говорим, что этот метод отвечает за POST-запросы (создание нового заказа)
        public async Task<ActionResult<Order>> CreateOrder(Order order) // Этот метод создает новый заказ
        {
            _context.Orders.Add(order); // Добавляем новый заказ в базу данных
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order); // Отправляем ответ, что заказ создан, с его данными
        }

        [HttpPut("{id}")] // Говорим, что этот метод отвечает за обновление данных заказа по его ID
        public async Task<IActionResult> UpdateOrder(int id, Order order) // Этот метод обновляет данные заказа
        {
            if (id != order.Id) // Если ID из адреса и ID из данных не совпадают...
            {
                return BadRequest(); // ...возвращаем ошибку
            }

            _context.Entry(order).State = EntityState.Modified;
            _context.Orders.Update(order);
            
            // Сообщаем базе данных, что данные заказа изменились
            try
            {
                await _context.SaveChangesAsync(); // Пробуем сохранить изменения
            }
            catch (DbUpdateConcurrencyException) // Если что-то пошло не так...
            {
                if (!OrderExists(id)) // ...проверяем, существует ли заказ
                {
                    return NotFound(); // Если заказа нет, говорим об этом
                }
                else
                {
                    throw; // Если проблема другая, выбрасываем ошибку
                }
            }

            return NoContent(); // Если всё прошло успешно, ничего не возвращаем
        }

        [HttpDelete("{id}")] // Говорим, что этот метод удаляет заказ по его ID
        public async Task<IActionResult> DeleteOrder(int id) // Этот метод удаляет заказ
        {
            var order = await _context.Orders.FindAsync(id); // Ищем заказ в базе данных
            if (order == null) // Если заказ не найден...
            {
                return NotFound(); // ...говорим, что его нет
            }

            _context.Orders.Remove(order); // Удаляем заказ из базы данных
            await _context.SaveChangesAsync(); // Сохраняем изменения

            return NoContent(); // Возвращаем, что всё прошло успешно
        }

        private bool OrderExists(int id) // Проверяем, есть ли заказ в базе данных
        {
            return _context.Orders.Any(e => e.Id == id); // Возвращаем "да", если заказ найден
        }
    }
}