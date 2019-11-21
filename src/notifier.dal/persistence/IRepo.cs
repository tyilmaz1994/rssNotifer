using MongoDB.Driver;
using notifier.dal.entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace notifier.dal.persistence
{
    public interface IRepo<T> where T : BaseEntity
    {
        IMongoCollection<T> Query();

        T Get(Expression<Func<T, bool>> expression);

        IList<T> GetList(Expression<Func<T, bool>> expression);

        T Update(T entity);

        T Add(T entity);

        void Delete(Expression<Func<T, bool>> expression);
    }
}
