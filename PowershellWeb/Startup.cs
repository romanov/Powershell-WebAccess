using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PowershellWeb
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var key = _configuration.GetValue<string>("key");
            PowershellSingleton.Instance.TrySetKey(key);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();


            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {


                app.UseRouter(routeBuilder =>
                {

                    routeBuilder.MapPost("/command", async (request, response, routeData) =>
                    {
                        var form = await request.ReadFormAsync();
                        string inputCommand = form["cmd"];
                        string inputKey = form["key"];

                      
                        CommandResult cr;

                        if (string.IsNullOrEmpty(inputKey) || inputKey != PowershellSingleton.Instance.Key)
                        {
                            cr = new CommandResult
                            {
                                Error = 1,
                                Result = "wrong key"
                            };
                        }
                        else
                        {
                            if (inputCommand == "Stop-WebAccess")
                                Environment.Exit(-1);

                            PowershellSingleton.Instance.InvokeCommand(inputCommand);

                            cr = new CommandResult
                            {
                                Error = 0,
                                Result = "ok"
                            };
                        }

                        var json = JsonSerializer.Serialize(cr);

                        response.ContentType = "text/json";
                        response.StatusCode = 200;


                        await response.WriteAsync(json);
                    });

                    routeBuilder.MapGet("/folder", async (request, response, routeData) =>
                    {
                      PowershellSingleton.Instance.InvokeCommand();
                      
                      var cr = new CommandResult
                      {
                        Error = 0,
                        Result = $"{PowershellSingleton.Instance.CurrentPath}>"
                      };

                      var json = JsonSerializer.Serialize(cr);

                      response.ContentType = "text/json";
                      response.StatusCode = 200;

                      await response.WriteAsync(json);
                    });

                    routeBuilder.MapGet("/status", async (request, response, routeData) =>
                    {
                      var json = ReadCommandResult(response);
                      await response.WriteAsync(json);
                    });
                });
            }
            else
            {

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Powershell Web is not available for your operating system.");
                    });

                });

            }


        }

        private static string ReadCommandResult(HttpResponse response)
        {
          var lastResult = PowershellSingleton.Instance.GetLastResult();

          var cr = new CommandResult
          {
            Count = lastResult.Item1,
            Error = 0,
            Result = lastResult.Item2
          };

          var json = JsonSerializer.Serialize(cr);
          response.ContentType = "text/json";
          response.StatusCode = 200;
          return json;
        }
    }
}
