using MassLab.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGlobalExceptionHandler()
    .AddCorsPolicy()
    .AddInfrastructure(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddDocumentation();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseCorsPolicy();
app.UseAuth();
app.MapEndpoints();
app.UseDocumentation();
app.UseHttpsRedirection();

app.Run();
