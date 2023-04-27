using Data_Layer.Repository;
using Infrastructure_Layer;
using Infrastructure_Layer.Models;
using Infrastructure_Layer.Repository;

namespace Data_Layer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MaindbContext _context;

        public UnitOfWork(MaindbContext context)
        {
            _context = context;
            Customers = new BaseRepository<Customer>(_context);

            Products = new BaseRepository<Product>(_context);
        }

        public IBaseRepository<Customer> Customers { get; private set; }

        public IBaseRepository<Product> Products { get; private set; }

        public IBaseRepository<Cart> Carts { get; private set; }

        public IBaseRepository<List<object>> objects { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
