using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Middleware
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Map("/skip", (skipApp) => skipApp.Run(async (context) => await context.Response.WriteAsync($"Skip the line!")));

            app.Use(async (context, next) => 
            {
                var value = context.Request.Query["value"].ToString();
                if (int.TryParse(value, out int intValue))
                {
                    await context.Response.WriteAsync($"You entered a number: {intValue}");
                }
                else
                {
                    context.Items["value"] = value;
                    await next();
                }
            });
            app.Use(async (context, next) => 
            {
                var value = context.Items["value"].ToString();
                if (int.TryParse(value, out int intValue))
                {
                    await context.Response.WriteAsync($"You entered a number: {intValue}");                    
                }
                else
                {
                    await next();
                }
            });
            app.Use(async (context, next) => 
            {
                var value = context.Items["value"].ToString();
                context.Items["value"] = value.ToUpper();
                await next();
            });
            app.Use(async (context, next) => {
                var value = context.Items["value"].ToString();
                context.Items["value"] = Regex.Replace(value, "(?<!^)[AEUI](?!$)", "*");
                await next();
            });
            app.Run(async (context) =>
            {
                var value = context.Items["value"].ToString();
                await context.Response.WriteAsync($"You entered a string: {value}");
            });
        }
    }
}
