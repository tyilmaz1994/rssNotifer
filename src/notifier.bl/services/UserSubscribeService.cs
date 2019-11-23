using notifier.dal.entities;
using notifier.dal.persistence;

namespace notifier.bl.services
{
    public class UserSubscribeService : AbstractService<UserSubscribe>, IUserSubscribeService
    {

        public UserSubscribeService(IRepo<UserSubscribe> repo) : base(repo)
        {
        }
    }

    public interface IUserSubscribeService : IAbstractService<UserSubscribe>
    {
        
    }
}
