using notifier.dal.entities;
using System.Collections.Generic;

namespace notifer.api.models
{
    public class BaseTransferModel<TEntity> where TEntity: BaseEntity
    {
        public TEntity Entity { get; set; }

        public IList<TEntity> EntityList { get; set; }
    }
}
