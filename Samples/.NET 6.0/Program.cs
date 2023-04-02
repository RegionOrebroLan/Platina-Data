using Microsoft.EntityFrameworkCore;
using RegionOrebroLan.Platina.Data.Builder;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSqlitePlatinaContext(options => options.UseSqlite(builder.Configuration.GetConnectionString("Platina")), ServiceLifetime.Transient, ServiceLifetime.Transient);

var app = builder.Build();

app.UsePlatinaContext();
app.UseRouting();
app.MapRazorPages();

app.Run();