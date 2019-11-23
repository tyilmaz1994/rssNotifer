using System;
using System.Linq.Expressions;
using notifier.dal.entities;
using notifier.dal.persistence;

namespace notifier.bl.services
{
    public class UserSubscribeService : AbstractService<UserSubscribe>, IUserSubscribeService
    {
        public UserSubscribeService(IRepo<UserSubscribe> repo) : base(repo)
        {
        }

        public override UserSubscribe Save(UserSubscribe entity)
        {
            return base.Save(entity);
        }

        public override void Delete(Expression<Func<UserSubscribe, bool>> expression)
        {
            base.Delete(expression);
        }
    }


    public interface IUserSubscribeService : IAbstractService<UserSubscribe>
    {
        
    }
}
