using Microsoft.EntityFrameworkCore;

namespace UserDB;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
              : base(options)
    {
    }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Notification> Notificaciones { get; set; }
}
