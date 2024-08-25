using Microsoft.AspNetCore.Identity;
using SDP.Data;

namespace Data.Repository{
    public class UserRepository : Repository<IdentityUser>, IUserRepository {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context):base(context) {
            _context = context;
        }
        public void Save() {
            _context.SaveChanges();
        }

    }
}
