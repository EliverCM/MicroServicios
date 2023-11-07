using UserDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Repositories;


namespace UserService.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext _context;
        private readonly IRepository<Cliente> _clienteRepository;
        public UserController(UserContext context, IRepository<Cliente> clienteRepository)
        {
            _context = context;
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Cliente>> GetCuentas()
        {
            var clientes = _clienteRepository.GetAll();
            return Ok(clientes);
        }


        [HttpGet("{id}")]
        public IActionResult GetCuenta(int id)
        {
            var cuenta = _clienteRepository.Get(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente(Cliente cliente)
        {
            try
            {
                // Verificar si el cliente ya existe (puedes usar un criterio como el número de identificación)
                var existingCliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Identificacion == cliente.Identificacion);

                if (existingCliente != null)
                {
                    return Conflict("El cliente ya existe"); // Devuelve 409 Conflict si el cliente ya existe
                }

                _clienteRepository.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCliente", new { id = cliente.ClienteID }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            // Buscar el cliente por su identificador
            var cliente = _clienteRepository.Get(id);

            if (cliente == null)
            {
                return NotFound(); // Devuelve 404 si el cliente no se encuentra
            }

            try
            {
                _clienteRepository.Delete(id);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si el cliente se eliminó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                return BadRequest("El identificador del cliente no coincide con los datos proporcionados.");
            }

            var existingCliente = _clienteRepository.Get(id);

            if (existingCliente == null)
            {
                return NotFound(); // Devuelve 404 si el cliente no se encuentra
            }

            try
            {
                // Actualiza las propiedades del cliente existente con los datos proporcionados
                existingCliente.Nombre = cliente.Nombre;
                existingCliente.Genero = cliente.Genero;
                existingCliente.Edad = cliente.Edad;
                existingCliente.Identificacion = cliente.Identificacion;
                existingCliente.Direccion = cliente.Direccion;
                existingCliente.Telefono = cliente.Telefono;

                _clienteRepository.Update(existingCliente);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si el cliente se actualizó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}


