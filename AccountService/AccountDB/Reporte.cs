using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDB
{
    public class Reporte
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<CuentaReporte> Cuentas { get; set; }
    }

    public class CuentaReporte
    {
        public int NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }
        public List<MovimientoReporte> Movimientos { get; set; }
    }

    public class MovimientoReporte
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int Valor { get; set; }
        public int Saldo { get; set; }
    }
}
