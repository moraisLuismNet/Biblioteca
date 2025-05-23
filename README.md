## Biblioteca
ASP.NET Core Web API Biblioteca

![Biblioteca](img/1.png)
![Biblioteca](img/2.png)


## Program
```cs 
builder.Services.AddDbContext<AlmacenContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);
``` 

## appsetting.Development.json
```cs 
{
  "ConnectionStrings": {
        "Connection": "Server=*;Database=Biblioteca;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
``` 

[DeepWiki moraisLuismNet/Biblioteca](https://deepwiki.com/moraisLuismNet/Biblioteca)