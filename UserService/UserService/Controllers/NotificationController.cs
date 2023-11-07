using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDB;
using Newtonsoft.Json;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private UserContext _context;
        public NotificationController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Notification>> GetNotificacion()
        {
            var Notificationes = await _context.Notificaciones.ToListAsync(); // Consulta la tabla Cuenta y recupera todos los registros
            
            if (Notificationes == null || Notificationes.Count == 0)
            {
                return NotFound(); // Devuelve 404 si no se encontraron registros
            }

            return Ok(Notificationes); // Devuelve la lista de cuentas en formato JSON
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotificacion(string message)
        {

            dynamic movimiento = Newtonsoft.Json.JsonConvert.DeserializeObject(message);
            string text = "Se ha realizado un " + movimiento.Tipo + " de " + movimiento.Valor + "USD en su cuenta.";
            // Asigna la fecha actual antes de guardar la notificación
            var notificacion = new Notification
            {
                Text = text,
                Fecha = DateTime.Now
            };
            
            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            // Devuelve la notificación creada con su ID generado
            return CreatedAtAction("GetNotificacion", new { id = notificacion.NotificationID }, notificacion);
        }
    }
}
