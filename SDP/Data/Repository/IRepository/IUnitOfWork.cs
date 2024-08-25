public interface IUnitOfWork {
    public IOrderRepository OrderRepository{get;}
    public IProductRepository ProductRepository{get;}
    public ICustomerRepository CustomerRepository{get;}
    public IStaffRepository StaffRepository{get;}
    public IUserRepository UserRepository{get;}

    public IPurchaseProductRepository PurchaseProductRepository{get;}

    public void Save();

}