

using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<AuctionSvcHttpClient>();

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
