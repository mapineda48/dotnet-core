using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Climate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * when executing the docker-compose up command, the database service should be expected.
             */
            string delay = Environment.GetEnvironmentVariable("APP_RUN_DELAY");

            if (delay != null)
            {
                int time = Int32.Parse(delay);

                Console.WriteLine($"Sleep {time}");
                Thread.Sleep(time);
            }


            /*
             * Run
             */
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
