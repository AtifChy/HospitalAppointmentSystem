using DAL.Repositories;

namespace BLL.Services;

public class GenericService<T> where T : class
{
    private readonly GenericRepository<T> _repository;

    public GenericService(GenericRepository<T> repository)
    {
        _repository = repository;
    }

    public List<T> GetAll()
    {
        return _repository.GetAll();
    }

    public T? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public void Add(T entity)
    {
        _repository.Add(entity);
    }

    public void Update(T entity)
    {
        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        _repository.Delete(id);
    }
}