using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ISetupRepository _setupRepository;

        public AdministrationService(ISetupRepository setupRepository)
        {
            _setupRepository = setupRepository;
        }


        /// <inheritdoc />
        public async Task<HttpContext> HandleAsync(HttpContext context, PathString path)
        {
            var response = context.Response;

            if (path.StartsWithSegments("/setup", out var setupPath))
            {                               
                return await HandleSetupsAsync(context, setupPath);
            }
  
            
            response.StatusCode = 200;

            await response.WriteAsync($"Admin area..");

            return context;
        }


        public async Task<HttpContext> HandleSetupsAsync(HttpContext context, PathString path)
        {
      
            if (path.StartsWithSegments("/create"))
            {
                var setup = await context.Request.ReadAsync<Setup>();
                _setupRepository.Add(setup);
                var json = JObject.FromObject(setup).ToString(Formatting.Indented);
                await context.Response.WriteAsync($"Setup created successfully!\n\n{json}");
                context.Response.StatusCode = (int) HttpStatusCode.Created;
                return context;
            }
            else if (path.StartsWithSegments("/list"))
            {
                var json = JArray.FromObject(_setupRepository.GetAll().ToList()).ToString(Formatting.Indented);
                await context.Response.WriteAsync(json);
                context.Response.StatusCode = (int) HttpStatusCode.OK;
                return context;
            }
            else
            {
                await context.Response.WriteAsync($"No api endpoint available for /setup/{path.Value}");
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return context;
            }
       
        }
    }
}
