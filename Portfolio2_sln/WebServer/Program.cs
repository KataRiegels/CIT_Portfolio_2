using DataLayer;
using DataLayer.DataServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IDataServiceNames, DataServiceNames>();
builder.Services.AddSingleton<IDataServiceTitles, DataServiceTitles>();
builder.Services.AddSingleton<IDataServiceUser, DataServiceUser>();
builder.Services.AddSingleton<IDataServiceSearches, DataServiceSearches>();

var app = builder.Build();

app.MapControllers();
app.Run();