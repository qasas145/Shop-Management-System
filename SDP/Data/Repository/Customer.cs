using SDP.Models;

using SDP.Data;

namespace Data.Repository{
    public class CustomerRepository : Repository<customer>,ICustomerRepository {
        private readonly ApplicationDbContext _context;
            public CustomerRepository(ApplicationDbContext context):base(context) {
                _context = context;
            }
            public void Save() {
                _context.SaveChanges();
            }

    }
}