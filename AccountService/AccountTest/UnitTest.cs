using System;
using AccountDB;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace AccountTest
{
    public class UnitTest
    {
        [Fact]
        public void CrearMovimiento_DebeTenerPropiedadesCorrectas()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AccountContext>()
                .UseInMemoryDatabase(databaseName: "Account")
                .Options;

            using (var context = new AccountContext(options))
            {
                var fecha = DateTime.Now;
                var tipo = "Deposito";
                var valor = 100;
                var saldo = 500;
                var numeroCuenta = 1;

                // Act
                var movimiento = new Movimiento
                {
                    Fecha = fecha,
                    Tipo = tipo,
                    Valor = valor,
                    Saldo = saldo,
                    NumeroCuenta = numeroCuenta
                };

                context.Movimientos.Add(movimiento);
                context.SaveChanges();

                // Assert
                var movimientoGuardado = context.Movimientos.Find(movimiento.MovimientoID);
                Assert.NotNull(movimientoGuardado);
                Assert.Equal(fecha, movimientoGuardado.Fecha);
                Assert.Equal(tipo, movimientoGuardado.Tipo);
                Assert.Equal(valor, movimientoGuardado.Valor);
                Assert.Equal(saldo, movimientoGuardado.Saldo);
                Assert.Equal(numeroCuenta, movimientoGuardado.NumeroCuenta);
            }
        }
    }
}