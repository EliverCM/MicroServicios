using Microsoft.EntityFrameworkCore;
using UserDB;

namespace UserTest
{
    public class UnitTest
    {

        [Fact]
        public void Cliente_PropiedadesDebenTenerValoresCorrectos()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: "Usuario")
            .Options;

            using (var context = new UserContext(options))
            {
                int clienteID = 999999999;
                string nombre = "Juan Perez";
                string genero = "Masculino";
                int edad = 30;
                string identificacion = "123456789";
                string direccion = "Calle Principal 123";
                string telefono = "123-456-7890";

                // Act
                var cliente = new Cliente
                {
                    ClienteID = clienteID,
                    Nombre = nombre,
                    Genero = genero,
                    Edad = edad,
                    Identificacion = identificacion,
                    Direccion = direccion,
                    Telefono = telefono
                };

                context.Clientes.Add(cliente);
                context.SaveChanges();

                // Assert
                var clienteGuardado = context.Clientes.Find(cliente.ClienteID);
                Assert.Equal(clienteID, clienteGuardado.ClienteID);
                Assert.Equal(nombre, clienteGuardado.Nombre);
                Assert.Equal(genero, clienteGuardado.Genero);
                Assert.Equal(edad, clienteGuardado.Edad);
                Assert.Equal(identificacion, clienteGuardado.Identificacion);
                Assert.Equal(direccion, clienteGuardado.Direccion);
                Assert.Equal(telefono, clienteGuardado.Telefono);
            }
        }
    }
}