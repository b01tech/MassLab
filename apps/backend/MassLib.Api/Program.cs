using MassLib.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorsPolicy()
    .AddInfrastructure(builder.Configuration)
    .AddDocumentation();

var app = builder.Build();

app.UseCorsPolicy();
app.MapEndpoints();
app.UseDocumentation();
app.UseHttpsRedirection();

app.Run();
