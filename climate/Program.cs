using System;
using System.Threading;
using Climate;
using Climate.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
 * Builder Application
 */
var builder = WebApplication.CreateBuilder(args);

/*
 * Services
 */
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

builder.Services.AddClimateContext(context =>
{
    context.Database.EnsureCreated();
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*
 * Configure App
 */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClimateApi v1"));

}
else
{
    app.UseExceptionHandler("/Error");
}

//app.UseHttpsRedirection();
//app.MapControllers();
//app.UseAuthorization();
//app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        // spa.UseReactDevelopmentServer(npmScript: "start");

        /*
         * https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-6.0&tabs=visual-studio#run-the-cra-server-independently
         */
        //spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
    }
});

app.Run();