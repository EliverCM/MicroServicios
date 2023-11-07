using UserDB;

namespace UserService.Repositories
{
    public class NotificationRepository : IRepository<Notification>
    {
        private readonly UserContext _context;

        public NotificationRepository(UserContext context)
        {
            _context = context;
        }

        public Notification Get(int id)
        {
            return _context.Notificaciones.Find(id);
        }

        public IEnumerable<Notification> GetAll()
        {
            return _context.Notificaciones.ToList();
        }

        public void Add(Notification entity)
        {
            _context.Notificaciones.Add(entity);
        }

        public void Update(Notification entity)
        {
            _context.Notificaciones.Update(entity);
        }

        public void Delete(int id)
        {
            var notificacion = _context.Notificaciones.Find(id);
            if (notificacion != null)
            {
                _context.Notificaciones.Remove(notificacion);
            }
        }

        // Otros métodos específicos si es necesario
    }
}
