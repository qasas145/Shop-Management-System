using SDP.Data;
using SDP.Models;

namespace Data.Repository{
    public class StaffRepository : Repository<staff>, IStaffRepository {
        private readonly ApplicationDbContext _context;
        public StaffRepository(ApplicationDbContext context):base(context) {
            _context = context;
        }
        public void Save() {
            _context.SaveChanges();
        }

    }
}
