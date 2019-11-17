using notifier.bl.enums;
using notifier.dal.entities;
using notifier.dal.persistence;
using notifier.tests.helpers;
using System;
using System.Linq;
using Xunit;

namespace notifier.tests.repos
{
    public abstract class BaseRepoTest<TEntity> : ApiTestServer
        where TEntity : BaseEntity
    {
        protected readonly IRepo<TEntity> Repo;
        public BaseRepoTest()
        {
            Repo = GetService<IRepo<TEntity>>();
        }

        [Fact]
        public void Get_Test()
        {
            var entity = Repo.Add(GenerateHelper.GenerateEntity<TEntity>());

            var gotEntity = Repo.Get(x => x.Id == entity.Id);

            Assert.NotNull(gotEntity);

            foreach (var item in (typeof(TEntity)).GetProperties())
            {
                if (item.PropertyType == typeof(DateTime))
                    continue;

                Assert.Equal(item.GetValue(entity), item.GetValue(gotEntity));
            }
        }

        [Fact]
        public void GetList_Test()
        {
            var entities = Repo.GetList(x => x.Active == (short)Active.Yes);

            Assert.NotNull(entities);

            var savedEntity = Repo.Add(GenerateHelper.GenerateEntity<TEntity>());

            var entitiesAfterSaved = Repo.GetList(x => x.Active == (short)Active.Yes);

            Assert.NotNull(entitiesAfterSaved);

            Assert.Equal(entities.Count + 1, entitiesAfterSaved.Count);

            var savedEntitiyAfterSave = entitiesAfterSaved.Where(x => x.Id == savedEntity.Id).FirstOrDefault();

            Assert.NotNull(savedEntitiyAfterSave);

            foreach (var item in (typeof(TEntity)).GetProperties())
            {
                if (item.PropertyType == typeof(DateTime))
                    continue;

                Assert.Equal(item.GetValue(savedEntitiyAfterSave), item.GetValue(savedEntity));
            }
        }

        [Fact]
        public void Add_Test()
        {
            var entities = Repo.GetList(x => x.Active == (short)Active.Yes);

            var savedEntity = Repo.Add(GenerateHelper.GenerateEntity<TEntity>());

            var entitiesAfterSaved = Repo.GetList(x => x.Active == (short)Active.Yes);

            Assert.NotNull(savedEntity);
            Assert.NotNull(savedEntity.Id);
            Assert.NotNull(entitiesAfterSaved);

            Assert.Equal(entities.Count + 1, entitiesAfterSaved.Count);
        }

        [Fact]
        public void Update_Test()
        {
            var savedEntity = Repo.Add(GenerateHelper.GenerateEntity<TEntity>());

            var gotSavedEntity = Repo.Get(x => x.Id == savedEntity.Id);

            var changedEntity = GenerateHelper.ChangeEntityValues<TEntity>(gotSavedEntity);

            var gotChangedEntity = Repo.Update(changedEntity);

            Assert.NotNull(gotSavedEntity);
            Assert.NotNull(gotChangedEntity);

            foreach (var item in (typeof(TEntity)).GetProperties())
            {
                if (item.PropertyType == typeof(DateTime))
                    continue;

                Assert.Equal(item.GetValue(gotSavedEntity), item.GetValue(gotChangedEntity));
            }
        }

        [Fact]
        public void Delete_Test()
        {
            var savedEntity = Repo.Add(GenerateHelper.GenerateEntity<TEntity>());

            Repo.Delete(x => x.Id == savedEntity.Id);

            var gotDeletedEntity = Repo.Get(x => x.Id == savedEntity.Id);

            Assert.Null(gotDeletedEntity);
        }
    }
}
