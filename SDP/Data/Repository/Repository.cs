using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SDP.Data;
namespace Data.Repository{
    public class  Repository<Type> : IRepository<Type>  where Type : class{
        private readonly ApplicationDbContext _context;
        internal DbSet<Type> dbSet;
        public Repository(ApplicationDbContext context) {
            _context = context;
            dbSet = this._context.Set<Type>();
        }
        public Type Get(Expression<Func<Type, bool>>? filter = null,string? includePropreties = null) {
            IQueryable<Type> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includePropreties)) {
                foreach(string prop in includePropreties.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }
        public IEnumerable<Type> GetAll(Expression<Func<Type, bool>> filter = null,string includePropreties = null) {
                IQueryable<Type> query = dbSet;
                if (filter != null) {
                    query = query.Where(filter);
                }
                if (!string.IsNullOrEmpty(includePropreties)) {
                    foreach(string prop in includePropreties.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
                        query = query.Include(prop);
                    }
                }
                return query.ToList();
        }
        public void Add(Type entity) {
            dbSet.Add(entity);
        }
        public void Update(Type entity) {
            dbSet.Update(entity);
        }
        public void Remove(Type entity) {
            dbSet.Remove(entity);
        }

    }
}