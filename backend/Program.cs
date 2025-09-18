

using jornal.Controllers;
using jornal.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

builder.Services.AddControllers();

// Configura o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // ou a porta que seu frontend está usando
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Adiciona o CORS antes do Authorization e MapControllers
app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.MapControllers();

var renderUserEndpoint = new UserEndpointRequest();
var renderPostEndpoint = new PostEndpointRequest();
renderUserEndpoint.AddRoute(app);
renderPostEndpoint.AddRoute(app);


app.Run();
