using notifier.dal.entities;
using notifier.dal.persistence;

namespace notifier.bl.services
{
    public class TelegramGroupService : AbstractService<TelegramGroup>, ITelegramGroupService
    {
        private readonly IRepo<TelegramGroup> _repo;

        public TelegramGroupService(IRepo<TelegramGroup> repo) : base(repo)
        {
            _repo = repo;
        }

        public TelegramGroup AddGroupIfNotExist(TelegramGroup input)
        {
            var chat = _repo.Get(x => x.ChatId == input.ChatId);
            if (chat == null)
                return _repo.Add(input);
            else return _repo.Update(chat);
        }
    }

    public interface ITelegramGroupService : IAbstractService<TelegramGroup>
    {
        TelegramGroup AddGroupIfNotExist(TelegramGroup input);
    }
}
