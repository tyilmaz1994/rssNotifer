using Microsoft.AspNetCore.Mvc;
using notifer.api.models;
using notifier.bl.enums;
using notifier.bl.services;
using notifier.dal.entities;
using System;
using System.Collections.Generic;

namespace notifer.api.Controllers
{
    public class AbstractController<TRequest, TResponse, TService, TEntity> : Controller
        where TRequest : BaseRequestModel<TEntity>
        where TResponse : BaseResponseModel<TEntity>
        where TService : IAbstractService<TEntity>
        where TEntity : BaseEntity
    {
        private readonly TService _service;

        public AbstractController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual ActionResult<IList<TEntity>> Get()
        {
            TResponse response = Activator.CreateInstance<TResponse>();
            response.EntityList = _service.GetList(x => x.Active == (short)Active.Yes);
            return Json(response);
        }

        [HttpGet("{id}")]
        public virtual ActionResult<IList<TEntity>> Get(string id)
        {
            TResponse response = Activator.CreateInstance<TResponse>();
            response.Entity = _service.Get(x => x.Id == id);
            return Json(response);
        }

        [HttpPost]
        public virtual ActionResult<TResponse> Post(TRequest request)
        {
            TResponse response = Activator.CreateInstance<TResponse>();

            if(request == null || request.Entity == null)
            {
                response.AddMessage(ApiResource.request_model_is_empty, isSuccessMessage: false);
            }
            else
            {
                if(string.IsNullOrEmpty(request.Entity.Id))
                {
                    response.Entity = _service.Save(request.Entity);
                }
                else
                {
                    var entity = _service.Get(x => x.Id == request.Entity.Id);
                    if (entity == null)
                    {
                        response.AddMessage(ApiResource.data_is_not_found, isSuccessMessage: false);
                    }
                    else
                    {
                        response.Entity = _service.Save(request.Entity);
                    }
                }
            }

            return Json(response);
        }

        [HttpPut]
        public virtual ActionResult<TResponse> Put(TRequest request)
        {
            TResponse response = Activator.CreateInstance<TResponse>();

            if (request == null || request.Entity == null || string.IsNullOrEmpty(request.Entity.Id))
            {
                response.AddMessage(ApiResource.request_model_is_not_found_or_data_is_not_found_by_given_id, isSuccessMessage: false);
            }
            else
            {
                var entity = _service.Get(x => x.Id == request.Entity.Id);

                if(entity == null)
                {
                    response.AddMessage(ApiResource.data_is_not_found, isSuccessMessage: false);
                }
                else
                {
                    response.Entity = _service.Save(entity);
                }
            }

            return Json(response);
        }

        [HttpDelete("{id}")]
        public virtual ActionResult<TResponse> Delete(string id)
        {
            TResponse response = Activator.CreateInstance<TResponse>();

            if (string.IsNullOrEmpty(id))
            {
                response.AddMessage(ApiResource.id_format_is_not_correct, isSuccessMessage: false);
            }
            else
            {
                TEntity entity = _service.Get(x => x.Id == id);
                if(entity == null)
                {
                    response.AddMessage(ApiResource.entity_not_found_by_given_id, isSuccessMessage: false);
                }
                else
                {
                    response.Entity = entity;
                    _service.Delete(x => x.Id == id);
                }
            }

            return response;
        }
    }
}