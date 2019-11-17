using Microsoft.AspNetCore.Mvc;
using notifer.api;
using notifer.api.Controllers;
using notifer.api.models;
using notifier.bl.enums;
using notifier.bl.services;
using notifier.dal.entities;
using notifier.tests.helpers;
using System;
using System.Linq;
using Xunit;

namespace notifier.tests.controllers
{
    public abstract class BaseControllerTest<TController, TService, TRequest, TResponse, TEntity> : ApiTestServer
        where TController : AbstractController<TRequest, TResponse, TService, TEntity>
        where TEntity : BaseEntity
        where TService : IAbstractService<TEntity>
        where TRequest : BaseRequestModel<TEntity>
        where TResponse : BaseResponseModel<TEntity>
    {
        [Fact]
        public void Get_Test()
        {
            var service = GetService<TService>();
            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Get().Result;
            Assert.NotNull(((JsonResult)result).Value);
        }

        [Fact]
        public void GetById_Test()
        {
            var service = GetService<TService>();

            var entityList = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entity;

            if (entityList.Any())
            {
                entity = entityList.FirstOrDefault();
            }
            else
            {
                entity = GenerateHelper.GenerateEntity<TEntity>();
                service.Save(entity);
                entity = service.GetList(x => x.Active == (short)Active.Yes).FirstOrDefault();
            }

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Get(entity.Id).Result;

            var gotEntity = ((BaseResponseModel<NotifierLog>)((JsonResult)result).Value).Entity;

            Assert.NotNull(gotEntity);
            Assert.Equal(gotEntity.Active, entity.Active);
            Assert.Equal(gotEntity.Id, entity.Id);
        }

        [Fact]
        public void Post_Test()
        {
            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            var service = GetService<TService>();

            baseRequest.Entity = GenerateHelper.GenerateEntity<TEntity>();

            var count = service.GetList(x => x.Active == (short)Active.Yes).Count;

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Post(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.True(!string.IsNullOrEmpty((response.Entity.Id)));

            var renewedCount = service.GetList(x => x.Active == (short)Active.Yes).Count;

            Assert.True(count < renewedCount);
        }

        [Fact]
        public void Post_null_entitiy_Test()
        {
            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            var service = GetService<TService>();

            var count = service.GetList(x => x.Active == (short)Active.Yes).Count;

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Post(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.request_model_is_empty);
        }

        [Fact]
        public void Post_null_request_Test()
        {
            var service = GetService<TService>();

            var count = service.GetList(x => x.Active == (short)Active.Yes).Count;

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Post(null).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.request_model_is_empty);
        }

        [Fact]
        public void Post_as_update_Test()
        {
            var service = GetService<TService>();
            var entities = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entitiy;

            if (entities.Any())
            {
                entitiy = entities.FirstOrDefault();
            }
            else
            {
                entitiy = service.Save(GenerateHelper.GenerateEntity<TEntity>());
            }

            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            baseRequest.Entity = GenerateHelper.ChangeEntityValues<TEntity>(entitiy);

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Post(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            var updatedLog = service.Get(x => x.Id == entitiy.Id);

            Assert.NotNull(response.Entity);
            Assert.NotNull(updatedLog);

            foreach (var item in typeof(TEntity).GetProperties())
            {
                if (item.PropertyType == typeof(DateTime))
                    continue;

                Assert.Equal(item.GetValue(entitiy), item.GetValue(response.Entity));
            }
        }

        [Fact]
        public void Post_as_update_entity_id_wrong_Test()
        {
            var service = GetService<TService>();
            var entities = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entity = GenerateHelper.GenerateEntity<TEntity>();
            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            entity.Id = new string('0', 24);
            baseRequest.Entity = entity;
            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Post(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.data_is_not_found);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public void Put_Test()
        {
            var service = GetService<TService>();
            var entities = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entitiy;

            if (entities.Any())
            {
                entitiy = entities.FirstOrDefault();
            }
            else
            {
                entitiy = service.Save(GenerateHelper.GenerateEntity<TEntity>());
            }

            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            baseRequest.Entity = GenerateHelper.ChangeEntityValues<TEntity>(entitiy);

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Put(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            var updatedLog = service.Get(x => x.Id == entitiy.Id);

            Assert.NotNull(response.Entity);
            Assert.NotNull(updatedLog);

            foreach (var item in typeof(TEntity).GetProperties())
            {
                if (item.PropertyType == typeof(DateTime))
                    continue;

                Assert.Equal(item.GetValue(entitiy), item.GetValue(response.Entity));
            }
        }

        [Fact]
        public void Put_null_entitiy_Test()
        {
            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            var service = GetService<TService>();

            var count = service.GetList(x => x.Active == (short)Active.Yes).Count;

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Put(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.request_model_is_not_found_or_data_is_not_found_by_given_id);
        }

        [Fact]
        public void Put_null_request_Test()
        {
            var service = GetService<TService>();

            var count = service.GetList(x => x.Active == (short)Active.Yes).Count;

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Put(null).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.request_model_is_not_found_or_data_is_not_found_by_given_id);
        }

        [Fact]
        public void Put_as_update_entity_id_wrong_Test()
        {
            var service = GetService<TService>();
            var entities = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entity = GenerateHelper.GenerateEntity<TEntity>();
            TRequest baseRequest = Activator.CreateInstance<TRequest>();
            entity.Id = new string('0', 24);
            baseRequest.Entity = entity;
            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Put(baseRequest).Result;

            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Null(response.Entity);
            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.data_is_not_found);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public void DeleteById_Test()
        {
            var service = GetService<TService>();

            var entities = service.GetList(x => x.Active == (short)Active.Yes);

            TEntity entitiy;

            if (entities.Any())
            {
                entitiy = entities.FirstOrDefault();
            }
            else
            {
                entitiy = service.Save(GenerateHelper.GenerateEntity<TEntity>());
            }

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Delete(entitiy.Id).Result;

            var deletedEntity = service.Get(x => x.Id == entitiy.Id);

            Assert.Null(deletedEntity);
        }

        [Fact]
        public void DeleteById_id_null_Test()
        {
            var service = GetService<TService>();

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Delete(null).Result;
            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.id_format_is_not_correct);
            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public void DeleteById_id_missing_Test()
        {
            var service = GetService<TService>();

            var controller = (AbstractController<TRequest, TResponse, TService, TEntity>)Activator.CreateInstance(typeof(TController), service);
            var result = controller.Delete(new string('0', 24)).Result;
            TResponse response = (TResponse)(((JsonResult)result).Value);

            Assert.Equal(response.Messages.FirstOrDefault(), ApiResource.entity_not_found_by_given_id);
            Assert.Null(response.Entity);
            Assert.Null(response.EntityList);
            Assert.False(response.IsSuccess);
        }
    }
}