using Microsoft.EntityFrameworkCore;
using Agape.Models.Extensions;
using Agape.Storage.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Registra el DbContext
var connectionStringDb = Environment.GetEnvironmentVariable("DATABASE_CONNECTION") ?? builder.Configuration.GetConnectionString("AgapeDatabase");
builder.Services.AddAppDbContext(connectionStringDb);

// Registra el Storage
var connectionStorage = Environment.GetEnvironmentVariable("STORAGE_URL") ?? builder.Configuration.GetConnectionString("StorageUrl");
builder.Services.AddStorageService(connectionStorage);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.InitializeDatabase();

app.Run();
