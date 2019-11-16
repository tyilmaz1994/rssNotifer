using notifier.dal.entities;

namespace notifer.api.models
{
    public class BaseRequestModel<TEntity> : BaseTransferModel<TEntity> where TEntity: BaseEntity
    {

    }
}
