using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PowershellWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create background thread with powershell console
            var server = new PowershellService();
            var _serverThread = new Thread(server.Main) { IsBackground = true };
            _serverThread.Start();

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
               {
                   

                   webBuilder.UseUrls("http://*:5000");

                   webBuilder.ConfigureKestrel(serverOptions =>
                   {
                       serverOptions.Limits.MaxConcurrentConnections = 1;
                   })
                   
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       config.AddCommandLine(args);
                   }).UseStartup<Startup>();

                   

               });


      


    }
}
