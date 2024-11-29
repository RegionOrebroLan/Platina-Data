using Microsoft.EntityFrameworkCore;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSqlitePlatinaContext(options => options.UseSqlite(builder.Configuration.GetConnectionString("Platina")), ServiceLifetime.Transient, ServiceLifetime.Transient);

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
	scope.ServiceProvider.GetRequiredService<PlatinaContext>().Database.Migrate();
}

app.UseRouting();
app.MapRazorPages();

app.Run();