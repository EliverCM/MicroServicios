using AccountDB;

namespace AccountService.Repositories
{
    public class AccountRepository : IRepository<Cuenta>
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context)
        {
            _context = context;
        }

        public Cuenta Get(int id)
        {
            return _context.Cuentas.Find(id);
        }

        public IEnumerable<Cuenta> GetAll()
        {
            return _context.Cuentas.ToList();
        }

        public void Add(Cuenta entity)
        {
            _context.Cuentas.Add(entity);
        }

        public void Update(Cuenta entity)
        {
            _context.Cuentas.Update(entity);
        }

        public void Delete(int id)
        {
            var cuenta = _context.Cuentas.Find(id);
            if (cuenta != null)
            {
                _context.Cuentas.Remove(cuenta);
            }
        }

        // Otros métodos específicos si es necesario
    }
}
