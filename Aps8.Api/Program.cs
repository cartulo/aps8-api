using Aps8.Api.Configurations;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.ConfigureServices(builder.Configuration);

services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins",
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("cidades", new OpenApiInfo
    {
        Title = "Cidades",
        Description = "Controller de Cidades.",
        Version = "cidades"
    });
});

var app = builder.Build();

app.UseCors("MyAllowedOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/cidades/swagger.json", "cidades");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
