using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reinventa.Aplicacion.Contratos;
using Reinventa.Aplicacion.NPS;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using Reinventa.Infraestructura.Auth;
using Reinventa.Infraestructura.Configuracion;
using Reinventa.Infraestructura.Email;
using Reinventa.Persistencia.Aprendizaje;
using Reinventa.Persistencia.BackOffice;
using Reinventa.Persistencia.HuellaCarbono;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using Reinventa.Seguridad.TokenSeguridad;
using Reinventa.Utilitarios.DTOS;
using ReinventaLab.Api.Middleware;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<NPS_Context>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConexionNPS"));
});

builder.Services.AddDbContext<LAB_Context>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConexionNPS"));
});


builder.Services.AddDbContext<Huella_Context>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConexionNPS"));
});

builder.Services.AddDbContext<SA_Context>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConexionNPS"));
});
builder.Services.AddDbContext<OFI_Context>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConexionNPS"));
});


//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Consultas.Encuestas.Manejador).Assembly));

builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Transacciones>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ListarProductosQuery).Assembly));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

var corsOrigin = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("Frontend"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazor", policy =>
    {
        policy.WithOrigins(corsOrigin!) 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();

var keyJWT = builder.Configuration["keyJWT:Clave"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        opt=> opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey= true,
            IssuerSigningKey= key,
            ValidateAudience= false,
            ValidateIssuer= false
        }
    );

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});
//Configracion de correo
builder.Services.Configure<AuthApiSettings>(
    builder.Configuration.GetSection("AuthApi"));
builder.Services.Configure<CorreoApiSettings>(
    builder.Configuration.GetSection("CorreoApi"));

builder.Services.AddHttpClient<ITokenService, TokenService>();
builder.Services.AddHttpClient<ICorreoService, CorreoApiService>();

//LOGS
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        "Logs/app-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.Use(async (context, next) =>
//{
//    // Añadir encabezado CSP
//    context.Response.Headers.Add("Content-Security-Policy", "script-src 'self';");

//    // Añadir encabezado X-Frame-Options
//    context.Response.Headers.Add("X-Frame-Options", "DENY");

//    await next();
//});



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reinventa API v1");
    });
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ManejadorErrorMiddleware>();
app.UseCors("PermitirBlazor");

app.MapControllers();

app.Run();
