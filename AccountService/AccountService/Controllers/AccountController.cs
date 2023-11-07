using AccountDB;
using AccountService.Repositories;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Controllers
{

    [ApiController]
    [Route("api/cuentas")]
    public class AccountController : ControllerBase
    {
        private AccountContext _context;
        private readonly IRepository<Cuenta> _accountRepository;

        public AccountController(AccountContext context, IRepository<Cuenta> accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Cuenta>> GetCuentas()
        {
            var cuentas = _accountRepository.GetAll();
            return Ok(cuentas); 
        }

        [HttpGet("{id}")]
        public IActionResult GetCuenta(int id)
        {
            var cuenta = _accountRepository.Get(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCuenta(Cuenta cuenta)
        {
            if (cuenta == null)
            {
                return BadRequest("Los datos de la cuenta no son válidos.");
            }
            try
            {
                _accountRepository.Add(cuenta);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCuenta), new { id = cuenta.NumeroCuenta }, cuenta);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                return StatusCode(500, $"Error interno del servidor: {innerException?.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = _accountRepository.Get(id); // Buscar la cuenta por su identificador
            if (cuenta == null)
            {
                return NotFound(); // Devuelve 404 si la cuenta no se encuentra
            }

            try
            {
                _accountRepository.Delete(id);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si la cuenta se eliminó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCuenta(int id, Cuenta cuenta)
        {
            if (id != cuenta.NumeroCuenta)
            {
                return BadRequest("El identificador de la cuenta no coincide con los datos proporcionados.");
            }

            var existingCuenta = _accountRepository.Get(id);
            if (existingCuenta == null)
            {
                return NotFound(); // Devuelve 404 si la cuenta no se encuentra
            }

            try
            {
                // Actualiza las propiedades de la cuenta existente con los datos proporcionados
                existingCuenta.NumeroCuenta = cuenta.NumeroCuenta;
                existingCuenta.TipoCuenta = cuenta.TipoCuenta;
                existingCuenta.SaldoInicial = cuenta.SaldoInicial;
                existingCuenta.Estado = cuenta.Estado;

                _accountRepository.Update(existingCuenta);
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 No Content si la cuenta se actualizó con éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
