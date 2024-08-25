using System.Linq.Expressions;

public interface IRepository<Type> {
    public Type Get(Expression<Func<Type, bool>>? filter = null,string? includePropreties = null);
    public IEnumerable<Type> GetAll(Expression<Func<Type, bool>>? filter = null,string? includePropreties = null);
    public void Update(Type entity);
    public void Add(Type entity);
    public void Remove(Type entity);
}