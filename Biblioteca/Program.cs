using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Configurar la cadena de conexi�n de manera global para que pueda ser utilizada a lo largo de toda la 
aplicaci�n, agregamos soporte para SQL Server indicando en la inyecci�n de dependencias que el contexto 
va a utilizar SQL Server con la siguiente cadena de conexi�n */
// Add services to the container.
builder.Services.AddDbContext<BibliotecaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    // Deshabilitar el tracking
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
