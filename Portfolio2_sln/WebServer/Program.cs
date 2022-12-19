using DataLayer;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebServer.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IDataServiceNames, DataServiceNames>();
builder.Services.AddSingleton<IDataServiceTitles, DataServiceTitles>();
builder.Services.AddSingleton<IDataServiceUser, DataServiceUser>();
builder.Services.AddSingleton<IDataServiceSearches, DataServiceSearches>();
builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticatedHandler>("BasicAuthentication", options => { });
builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, AdminAuthenticatedHandler>("AdminAuthentication", options => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
    options.AddPolicy("AdminAuthentication", new AuthorizationPolicyBuilder("AdminAuthentication").RequireAuthenticatedUser().Build());

});



var app = builder.Build();
app.UseCors(
    options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
);
app.UseAuthorization();

app.MapControllers();
app.Run();