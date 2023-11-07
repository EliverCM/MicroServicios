using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDB
{
    public class Movimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovimientoID { get; set; }
        public DateTime Fecha { get; set; }
        public string? Tipo { get; set; }
        public int Valor { get; set; }
        public int Saldo { get; set; }
        public int NumeroCuenta { get; set; }
        [ForeignKey("NumeroCuenta")]
        public virtual Cuenta Cuenta { get; set; }

    }
    public class MovimientoDTO
    {
        public DateTime Fecha { get; set; }
        public string? Tipo { get; set; }
        public int Valor { get; set; }
        public int NumeroCuenta { get; set; }
        public int? ClientID { get; set; }

    }

    public class RabbitMQConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
