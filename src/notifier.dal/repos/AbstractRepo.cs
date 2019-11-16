using MongoDB.Driver;
using notifier.dal.context;
using notifier.dal.entities;
using notifier.dal.extensions;
using notifier.dal.persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace notifier.dal.repos
{
    public class AbstractRepo<T> : IRepo<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _mongoCollection = null;

        public AbstractRepo(INotifierDbContext dbContext)
        {
            _mongoCollection = dbContext.OpenConnection<T>();
        }

        public T Add(T entity)
        {
            _mongoCollection.InsertOne(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _mongoCollection.ReplaceOne(x => x.Id == entity.Id, entity);
            return entity;
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            _mongoCollection.DeleteOne(expression);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _mongoCollection.Find(expression).FirstOrDefault();
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression)
        {
            return _mongoCollection.Find(expression).ToList();
        }
    }
}