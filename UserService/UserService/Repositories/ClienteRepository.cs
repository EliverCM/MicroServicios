using UserDB;

namespace UserService.Repositories
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly UserContext _context;

        public ClienteRepository(UserContext context)
        {
            _context = context;
        }

        public Cliente Get(int id)
        {
            return _context.Clientes.Find(id);
        }

        public IEnumerable<Cliente> GetAll()
        {
            return _context.Clientes.ToList();
        }

        public void Add(Cliente entity)
        {
            _context.Clientes.Add(entity);
        }

        public void Update(Cliente entity)
        {
            _context.Clientes.Update(entity);
        }

        public void Delete(int id)
        {
            var cuenta = _context.Clientes.Find(id);
            if (cuenta != null)
            {
                _context.Clientes.Remove(cuenta);
            }
        }

        // Otros métodos específicos si es necesario
    }
}
