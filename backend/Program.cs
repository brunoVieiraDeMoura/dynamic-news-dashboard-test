

using jornal.Controllers;
using jornal.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

builder.Services.AddControllers();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var renderUserEndpoint = new UserEndpointRequest();
var renderPostEndpoint = new PostEndpointRequest();
renderUserEndpoint.AddRoute(app);
renderPostEndpoint.AddRoute(app);


app.Run();
