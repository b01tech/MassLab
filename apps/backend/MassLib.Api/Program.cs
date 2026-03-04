using MassLib.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddDocumentation();

var app = builder.Build();

app.MapEndpoints();
app.UseDocumentation();
app.UseHttpsRedirection();

app.Run();
