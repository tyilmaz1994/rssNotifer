using notifier.dal.entities;
using notifier.dal.persistence;

namespace notifier.bl.services
{
    public class UserRssService : AbstractService<UserRss>, IUserRssService
    {
        private readonly IRepo<UserRss> _repo;

        public UserRssService(IRepo<UserRss> repo) : base(repo)
        {
            _repo = repo;
        }

        public UserRss AddRssIfNotExist(UserRss input)
        {
            var rss = _repo.Get(x => x.UserId == input.UserId && x.Url == input.Url);
            if (rss == null)
                return _repo.Add(input);
            else return _repo.Update(rss);
        }
    }

    public interface IUserRssService : IAbstractService<UserRss>
    {
        UserRss AddRssIfNotExist(UserRss input);
    }
}
