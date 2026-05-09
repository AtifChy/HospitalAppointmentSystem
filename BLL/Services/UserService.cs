using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class UserService : GenericService<User>
{
    public UserService(UserRepository repository) : base(repository)
    {
    }
}