using notifier.dal.entities;
using notifier.dal.persistence;

namespace notifier.bl.services
{
    public class UserService : AbstractService<User>, IUserService
    {
        private readonly IRepo<User> _repo;

        public UserService(IRepo<User> repo) : base(repo)
        {
            _repo = repo;
        }

        public User AddUserIfNotExist(User input)
        {
            var user = _repo.Get(x => x.TelegramId == input.TelegramId);
            if (user == null)
                return _repo.Add(input);
            else return _repo.Update(user);
        }
    }

    public interface IUserService : IAbstractService<User>
    {
        User AddUserIfNotExist(User input);
    }
}