using jornal.Controllers;
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

// Cria o builder da aplica��o ASP.NET Core, respons�vel por configurar servi�os e o pipeline
var builder = WebApplication.CreateBuilder(args);

// Cria autentica��o por meio de JWT
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

// Confugura enum e converte para string ou ao contrario OBS: nao esta sendo usado
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


// Configura o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL do frontend
                  .AllowAnyHeader()                     // Permite qualquer header HTTP
                  .AllowAnyMethod();                    // Permite qualquer m�todo (GET, POST, PUT, DELETE)
        });
});

// Cria��o do app a partir do builder
var app = builder.Build();


// Garante que o banco de dados seja criando ao iniciar a aplica��o
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Cria o arquivo users.db se n�o existir
}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Adiciona o CORS antes do Authorization e MapControllers
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

// Mapeia controllers para responder as rotas configuradas
app.MapControllers();


// Registra endpoints adicionais, definidos manualmente nas classes de Request
var renderUserEndpoint = new UserEndpointRequest();
var renderPostEndpoint = new PostEndpointRequest();
renderUserEndpoint.AddRoute(app); // Adiciona rotas personalizadas de usu�rio
renderPostEndpoint.AddRoute(app); // Adiciona rotas personalizadas de post

// Iicia a aplica��o e come�a a escutar requisi��es
app.Run();
