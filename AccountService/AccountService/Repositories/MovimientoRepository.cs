using AccountDB;

namespace AccountService.Repositories
{
    public class MovimientoRepository : IRepository<Movimiento>
    {
        private readonly AccountContext _context;

        public MovimientoRepository(AccountContext context)
        {
            _context = context;
        }

        public Movimiento Get(int id)
        {
            return _context.Movimientos.Find(id);
        }

        public IEnumerable<Movimiento> GetAll()
        {
            return _context.Movimientos.ToList();
        }

        public void Add(Movimiento entity)
        {
            _context.Movimientos.Add(entity);
        }

        public void Update(Movimiento entity)
        {
            _context.Movimientos.Update(entity);
        }

        public void Delete(int id)
        {
            var movimiento = _context.Movimientos.Find(id);
            if (movimiento != null)
            {
                _context.Movimientos.Remove(movimiento);
            }
        }

        // Otros métodos específicos si es necesario
    }
}
