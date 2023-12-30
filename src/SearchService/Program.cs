

using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try{
    await DbInnitializer.InitDb(app);
}
catch(System.Exception ex){
    Console.WriteLine(ex.Message);
}

app.Run();
