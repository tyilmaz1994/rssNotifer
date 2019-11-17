using notifier.bl.enums;
using notifier.bl.services;
using notifier.dal.entities;
using notifier.tests.helpers;
using System;
using System.Linq;
using Xunit;

namespace notifier.tests.services
{
    public abstract class BaseServiceTest<TService, TEntity> : ApiTestServer 
        where TService : IAbstractService<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly TService Service;

        public BaseServiceTest()
        {
            Service = GetService<TService>();
        }

        [Fact]
        public void Get_Test()
        {
            var entity = Service.Save(GenerateHelper.GenerateEntity<TEntity>());

            var gotEntity = Service.Get(x => x.Id == entity.Id);

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
            var entities = Service.GetList(x => x.Active == (short)Active.Yes);

            Assert.NotNull(entities);

            var savedEntity = Service.Save(GenerateHelper.GenerateEntity<TEntity>());

            var entitiesAfterSaved = Service.GetList(x => x.Active == (short)Active.Yes);

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
        public void Save_as_add_Test()
        {
            var entities = Service.GetList(x => x.Active == (short)Active.Yes);

            var savedEntity = Service.Save(GenerateHelper.GenerateEntity<TEntity>());

            var entitiesAfterSaved = Service.GetList(x => x.Active == (short)Active.Yes);

            Assert.NotNull(savedEntity);
            Assert.NotNull(savedEntity.Id);
            Assert.NotNull(entitiesAfterSaved);

            Assert.Equal(entities.Count + 1, entitiesAfterSaved.Count);
        }

        [Fact]
        public void Save_as_update_Test()
        {
            var savedEntity = Service.Save(GenerateHelper.GenerateEntity<TEntity>());

            var gotSavedEntity = Service.Get(x => x.Id == savedEntity.Id);

            var changedEntity = GenerateHelper.ChangeEntityValues<TEntity>(gotSavedEntity);

            var gotChangedEntity = Service.Save(changedEntity);

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
            var savedEntity = Service.Save(GenerateHelper.GenerateEntity<TEntity>());

            Service.Delete(x => x.Id == savedEntity.Id);

            var gotDeletedEntity = Service.Get(x => x.Id == savedEntity.Id);

            Assert.Null(gotDeletedEntity);
        }
    }
}