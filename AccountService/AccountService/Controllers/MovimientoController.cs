using AccountDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using AccountService.Services;
using AccountService.Repositories;

namespace AccountService.Controllers
{
    [Route("api/movimientos")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private AccountContext _context;
        private readonly RabbitMQService _rabbitMQService;
        private readonly IRepository<Movimiento> _movimientoRepository;
        private readonly IRepository<Cuenta> _accountRepository;
        public MovimientoController(
            AccountContext context,
            RabbitMQService rabbitMQService,
            IRepository<Movimiento> movimientoRepository,
            IRepository<Cuenta> accountRepository
            )
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
            _movimientoRepository = movimientoRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult GetMovimientos()
        {
            var movimientos = _movimientoRepository.GetAll();
            var cuentas = _accountRepository.GetAll();

            var movimientosConCuentas = movimientos.Join(
                cuentas,
                movimiento => movimiento.NumeroCuenta,  // La propiedad que representa la clave de relación en Movimiento
                cuenta => cuenta.NumeroCuenta,                // La propiedad que representa la clave de relación en Cuenta
                (movimiento, cuenta) => new
                {
                    Movimiento = movimiento,
                    Cuenta = cuenta
                })
                .Select(result => result.Movimiento)
                .ToList();

            return Ok(movimientosConCuentas);
           

        }

        [HttpGet("{id}")]
        public IActionResult GetMovimiento(int id)
        {
            var movimiento = _movimientoRepository.Get(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return Ok(movimiento);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovimiento(MovimientoDTO movimiento)
        {
            try
            {
                // Validar que el tipo de movimiento sea "Deposito" o "Retiro"
                if (movimiento.Tipo != "Deposito" && movimiento.Tipo != "Retiro")
                {
                    return BadRequest("Tipo de movimiento no válido");
                }

                // Validar si existe la cuenta para realizar el movimiento
                var cuenta = _accountRepository.Get(movimiento.NumeroCuenta);
                if (cuenta == null)
                {
                    return NotFound("Cuenta no encontrada");
                }
             

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Comprobar si el saldo es suficiente para los movimientos de retiro
                        if (movimiento.Tipo == "Retiro" && movimiento.Valor > cuenta.SaldoInicial)
                        {
                            return BadRequest("Saldo no disponible");
                        }
                        if (movimiento.Valor < 0)
                        {
                            movimiento.Tipo = "Retiro";
                            movimiento.Valor = movimiento.Valor * -1;
                        }
                        // Actualizar el saldo de la cuenta según el tipo de movimiento
                        if (movimiento.Tipo == "Deposito")
                        {
                            cuenta.SaldoInicial += movimiento.Valor;
                        }
                        else if (movimiento.Tipo == "Retiro")
                        {
                            cuenta.SaldoInicial -= movimiento.Valor;
                        }

                        var movimientos = new Movimiento
                        {
                            Fecha = DateTime.Now,
                            Tipo = movimiento.Tipo,
                            Valor = movimiento.Valor,
                            Saldo = cuenta.SaldoInicial,  // Ajusta el saldo según calculo
                            NumeroCuenta = movimiento.NumeroCuenta,
                            Cuenta = cuenta
                        };

                        _accountRepository.Update(cuenta);
                        _movimientoRepository.Add(movimientos);
                        // Guardar cambios en la base de datos
                        await _context.SaveChangesAsync();

                        _rabbitMQService.EnviarMensaje(movimiento);
                        // Confirmar la transacción
                        transaction.Commit();
                        return CreatedAtAction("GetMovimiento", new { id = movimientos.MovimientoID }, movimientos);
                    }
                    catch (Exception ex)
                    {
                        // En caso de error, revertir la transacción
                        transaction.Rollback();
                        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = _movimientoRepository.Get(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            try
            {
                _movimientoRepository.Delete(id);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si la cuenta se eliminó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovimiento(int id, Movimiento movimiento)
        {
            var existingMovimiento = _movimientoRepository.Get(id);
            if (existingMovimiento == null)
            {
                return NotFound(); // Devuelve 404 si el movimiento no se encuentra
            }

            try
            {
                // Actualiza las propiedades del movimiento existente con los datos proporcionados
                existingMovimiento.Fecha = movimiento.Fecha;
                existingMovimiento.Tipo = movimiento.Tipo;
                existingMovimiento.Valor = movimiento.Valor;
                existingMovimiento.Saldo = movimiento.Saldo;

                _movimientoRepository.Update(existingMovimiento);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si el movimiento se actualizó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
