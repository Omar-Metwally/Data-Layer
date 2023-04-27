using Infrastructure_Layer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data_Layer.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected MaindbContext _context;

        public BaseRepository(MaindbContext context)
        {
            _context = context;
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetByID(int id)
        {
            return _context.Set<T>().Find(id);
        }


        public T? Upsert(int? id, T entity)
        {
            if (id == null)
            {
                _context.Set<T>().Add(entity);
                return entity;
            }
            else
            {
                _context.Set<T>().Update(entity);
                return entity;
            }
        }
        public T? Upsert(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                var entity = _context.Set<T>().Find(id);
                return entity;
            }
        }
        public T? Details(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                var entity = _context.Set<T>().Find(id);
                return entity;
            }
        }
        public T? Delete(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                var entity = _context.Set<T>().Find(id);
                _context.Set<T>().Remove(entity);
                return null;
            }
        }
        public T? Index()
        {
            var query = from c in _context.Categorys
                        join p in _context.Products on c.CategoryId equals p.CategoryId
                        join m in _context.MeasuresOfScales on p.MeasureOfScaleId equals m.MeasureOfScaleId
                        select new
                        {
                            CategoryName = c.CategoryName,
                            ProductName = p.ProductName,
                            ProductId = p.ProductId,
                            Price = p.Price,
                            Instock = p.Instock,
                            InstockQty = p.InstockQty,
                            MeasureOfScale = m.MeasureOfScale
                        };
            return (T?)query;
        }

        public List<object> GetQuery(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList<object>();
            }
            else
            {
                return query.ToList<object>();
            }
        }
    }
}
