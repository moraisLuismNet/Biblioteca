## Biblioteca
ASP.NET Core Web API Biblioteca

![Biblioteca](img/1.png)
![Biblioteca](img/2.png)


## Program
``` 
builder.Services.AddDbContext<AlmacenContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);
``` 

## appsetting.Development.json
``` 
{
  "ConnectionStrings": {
        "Connection": "Server=W11P0RT;Database=Biblioteca;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
``` 
