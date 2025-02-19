using Biblioteca.AutoMappers;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;
using Biblioteca.Services;
using Biblioteca.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/* Configurar la cadena de conexión de manera global para que pueda ser utilizada a lo largo de toda la 
aplicación, agregamos soporte para SQL Server indicando en la inyección de dependencias que el contexto 
va a utilizar SQL Server con la siguiente cadena de conexión */
// Add services to the container.
builder.Services.AddDbContext<BibliotecaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    // Deshabilitar el tracking
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Evitar referencias circulares al utilizar include en los controllers
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configurar seguridad
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(builder.Configuration["ClaveJWT"]))
               });

// Configurar la seguridad en Swagger 
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Autenticación JWT usando el esquema Bearer. \r\n\r " +
        "Ingresa la palabra 'Bearer' seguido de un espacio y el token de autenticación",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

//Soporte para autenticación con .NET Identity
//builder.Services.AddIdentity<AppUsuario, IdentityRole>()
//    .AddEntityFrameworkStores<BibliotecaContext>()
//    .AddDefaultTokenProviders();


builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IEditorialService, EditorialService>();
builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddTransient<OperacionesService>();
builder.Services.AddTransient<IGestorArchivos, GestorArchivos>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataProtection();

// Validators
builder.Services.AddScoped<IValidator<AutorInsertDTO>, AutorInsertValidator>();
builder.Services.AddScoped<IValidator<AutorUpdateDTO>, AutorUpdateValidator>();
builder.Services.AddScoped<IValidator<EditorialInsertDTO>, EditorialInsertValidator>();
builder.Services.AddScoped<IValidator<EditorialUpdateDTO>, EditorialUpdateValidator>();
builder.Services.AddScoped<IValidator<LibroInsertDTO>, LibroInsertValidator>();
builder.Services.AddScoped<IValidator<LibroUpdateDTO>, LibroUpdateValidator>();

// Repository
builder.Services.AddScoped<ILibroRepository, LibroRepository>();
builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<IEditorialRepository, EditorialRepository>();

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

// Soporte para autenticación
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
