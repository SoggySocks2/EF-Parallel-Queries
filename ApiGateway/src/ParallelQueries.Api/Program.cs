using Microsoft.OpenApi.Models;
using ParallelQueries.CoreApp.Api.Setup;
using ParallelQueries.SharedKernel.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Bind(CoreAppInfrastructureConfig.CONFIG_NAME, CoreAppInfrastructureConfig.Instance);
await builder.Services.AddCoreAppServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Threads App",
        Version = "v1",
    });
    c.EnableAnnotations();
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();