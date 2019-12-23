using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Extensions.Responses;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    internal class ScopedHttpContextRepository : IScopeRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScopedHttpContextRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public Response<T> Get<T>(string key)
        {
            if (_httpContextAccessor.HttpContext?.Items is null)
            {
                return new ErrorResponse<T>(new NoDataFoundError($"Unable to find data for key: {key} because HttpContext or HttpContext.Items is null."));
            }
            
            if(_httpContextAccessor.HttpContext.Items.TryGetValue(key, out var value))
            {
                var item = (T) value;
                return new OkResponse<T>(item);
            }
            
            return new ErrorResponse<T>(new NoDataFoundError($"Unable to find data for key: {key}."));
        }


        /// <inheritdoc />
        public Response Add<T>(string key, T value)
        {
            try
            {
                // if (Items is null)
                // {
                //     return new ErrorResponse(new AddDataFailedError($"Unable to add data because HttpContext or HttpContext.Items is null. Key={key};"));
                // }

                _httpContextAccessor.HttpContext.Items.Add(key, value);
                
                return new OkResponse();
            }
            catch (Exception exception)
            {
                return new ErrorResponse(new AddDataFailedError($"Unable to add data. Key={key}; Exception={exception.Message};"));

            } 
        }

        /// <inheritdoc />
        public Response Update<T>(string key, T value)
        {
            try
            {
                // if (Items is null)
                // {
                //     return new ErrorResponse(new AddDataFailedError($"Unable to update data because HttpContext or HttpContext.Items is null. Key={key};"));
                // }
                
                _httpContextAccessor.HttpContext.Items[key] = value;
                return new OkResponse();
            }
            catch (Exception exception)
            {
                return new ErrorResponse(new AddDataFailedError($"Unable to update data. Key={key}; Exception={exception.Message};"));

            }
        }
    }
}
