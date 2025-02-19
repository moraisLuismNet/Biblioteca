using Biblioteca.AutoMappers;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;
using Biblioteca.Services;
using Biblioteca.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

// Validators
builder.Services.AddScoped<IValidator<AutorInsertDTO>, AutorInsertValidator>();
builder.Services.AddScoped<IValidator<AutorUpdateDTO>, AutorUpdateValidator>();
builder.Services.AddScoped<IValidator<EditorialInsertDTO>, EditorialInsertValidator>();
builder.Services.AddScoped<IValidator<EditorialUpdateDTO>, EditorialUpdateValidator>();
builder.Services.AddScoped<IValidator<LibroInsertDTO>, LibroInsertValidator>();
builder.Services.AddScoped<IValidator<LibroUpdateDTO>, LibroUpdateValidator>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IGestorArchivos, GestorArchivos>();
builder.Services.AddTransient<OperacionesService>();

// Repository
builder.Services.AddScoped<IRepository<Autor>, AutorRepository>();
builder.Services.AddScoped<IRepository<Editorial>, EditorialRepository>();
builder.Services.AddScoped<IRepository<Libro>, LibroRepository>();



// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container
builder.Services.AddScoped<ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>, AutorService>();
builder.Services.AddScoped<ICommonService<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>, EditorialService>();
builder.Services.AddScoped<ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO>, LibroService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
