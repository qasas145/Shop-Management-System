using SDP.Data;
using SDP.Models;

namespace Data.Repository {

    public class UnitOfWork : IUnitOfWork {
        private readonly ApplicationDbContext context;
        
        public IOrderRepository OrderRepository{get;private set;}
        public IProductRepository ProductRepository{get;private set;}
        public ICustomerRepository CustomerRepository{get;private set;}
        public IStaffRepository StaffRepository{get;private set;}
        public IUserRepository UserRepository{get;private set;}

        public IPurchaseProductRepository PurchaseProductRepository{get;set;}
        public UnitOfWork(ApplicationDbContext _context) {
            context = _context;
            UserRepository = new UserRepository(context);
            StaffRepository = new StaffRepository(context);
            ProductRepository = new ProductRepository(context);
            CustomerRepository = new CustomerRepository(context);
            OrderRepository = new OrderRepository(context);
        }
        public void Save() {
            context.SaveChanges();
        }

    }

}