using AccountDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccountService.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private AccountContext _context;
        public ReportesController(AccountContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GenerarReporteAsync(DateTime fechaInicio, DateTime fechaFin, int clienteId)
        {
            try
            {
                var httpClient = new HttpClient();
                var clienteResponse = await httpClient.GetAsync($"https://localhost:7275/api/clientes/{clienteId}");
                if (clienteResponse.IsSuccessStatusCode)
                {
                    var clienteData = await clienteResponse.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<dynamic>(clienteData);

                    var reporte = _context.Cuentas
                        .Where(c => c.ClientID == clienteId)
                        .Select(c => new CuentaReporte
                        {
                            NumeroCuenta = c.NumeroCuenta,
                            Saldo = c.SaldoInicial,
                            Movimientos = _context.Movimientos
                                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin && m.Cuenta.ClientID == clienteId && m.Cuenta.NumeroCuenta == c.NumeroCuenta)
                                .Select(m => new MovimientoReporte
                                {
                                    Fecha = m.Fecha,
                                    Tipo = m.Tipo,
                                    Valor = m.Valor,
                                    Saldo = m.Saldo
                                })
                                .ToList()
                        })
                        .ToList();

                    var reporteFinal = new Reporte
                    {
                        ClienteId = clienteId,
                        NombreCliente = cliente.nombre, // Suponiendo que el nombre del cliente se encuentra en la propiedad "Nombre"
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        Cuentas = reporte
                    };

                    // Devuelve el informe en formato JSON
                    return Ok(reporteFinal);
                }
                else
                {
                    // Manejar el caso en el que no se pudo obtener la información del cliente
                    return NotFound("No funciona");
                }
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                return StatusCode(500, $"Error interno del servidor: {innerException?.Message}");
            }
        }

    }
}
