using Microsoft.EntityFrameworkCore;

namespace AccountDB
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) 
            : base(options) 
        { 

        }
        public DbSet<Cuenta> Cuentas { get; set;}
        public DbSet<Movimiento> Movimientos { get; set;}
    }
}