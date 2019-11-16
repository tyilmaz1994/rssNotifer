using notifier.dal.entities;
using notifier.dal.persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace notifier.bl.services
{
    public abstract class AbstractService<T> : IAbstractService<T> where T : BaseEntity
    {
        private readonly IRepo<T> _repo = null;

        public AbstractService(IRepo<T> repo)
        {
            _repo = repo;
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _repo.Get(expression);
        }

        public IList<T> GetList(Expression<Func<T, bool>> expression)
        {
            return _repo.GetList(expression);
        }

        /// <summary>
        /// Add or Update record depends on its Id.
        /// If id is null then record will be saved.
        /// If Id is not null then record will be updated.
        /// </summary>
        /// <param name="entity">the entity of save or update action</param>
        /// <returns>itself after action perform</returns>
        public T Save(T entity)
        {
            if(entity.Id == null)
                return _repo.Add(entity);
            else return _repo.Update(entity);
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            _repo.Delete(expression);
        }
    }

    public interface IAbstractService<T> where T: BaseEntity
    {
        T Get(Expression<Func<T, bool>> expression);

        IList<T> GetList(Expression<Func<T, bool>> expression);

        T Save(T entity);

        void Delete(Expression<Func<T, bool>> expression);
    }
}