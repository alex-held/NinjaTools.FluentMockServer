using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ISetupService _setupService;

        public AdministrationService(ISetupService setupService)
        {
            _setupService = setupService;
        }


        /// <inheritdoc />
        [ItemNotNull]
        public async Task<HttpContext> HandleAsync([NotNull] HttpContext context, PathString path)
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


        [ItemNotNull]
        public async Task<HttpContext> HandleSetupsAsync([NotNull] HttpContext context, PathString path)
        {
      
            if (path.StartsWithSegments("/create"))
            {
                var setup = await context.Request.ReadAsync<Setup>();
                _setupService.Add(setup);
                var json = JObject.FromObject(setup).ToString(Formatting.Indented);
                context.Response.StatusCode = (int) HttpStatusCode.Created;
                await context.Response.WriteAsync($"Setup created successfully!\n\n{json}");
                return context;
            }
            else if (path.StartsWithSegments("/list"))
            {
                var json = JArray.FromObject(_setupService.GetAll().ToList()).ToString(Formatting.Indented);
                context.Response.StatusCode = (int) HttpStatusCode.OK;
                await context.Response.WriteAsync(json);
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
