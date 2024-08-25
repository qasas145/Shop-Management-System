using SDP.Models;

using SDP.Data;

namespace Data.Repository
{
    public class ProductRepository : Repository<product>,IProductRepository {
        private readonly ApplicationDbContext _context;
            public ProductRepository(ApplicationDbContext context):base(context) {
                _context = context;
            }
            public void Save() {
                _context.SaveChanges();
            }

    }
}