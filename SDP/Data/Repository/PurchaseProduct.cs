using SDP.Data;
using SDP.Models;

namespace Data.Repository
{
    public class PurchaseProductRepository : Repository<PurchaseProduct> , IPurchaseProductRepository {
        private readonly ApplicationDbContext _context;
        public PurchaseProductRepository(ApplicationDbContext context):base(context) {
            _context = context;
        }
        public void Save() {
            _context.SaveChanges();
        }

    }
}
