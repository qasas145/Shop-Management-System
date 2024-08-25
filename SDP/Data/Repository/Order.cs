using SDP.Models;

using SDP.Data;
namespace Data.Repository{

    public class OrderRepository : Repository<Order>,IOrderRepository {
        private readonly ApplicationDbContext _context;
            public OrderRepository(ApplicationDbContext context):base(context) {
                _context = context;
            }
            public void Save() {
                _context.SaveChanges();
            }

    }
}