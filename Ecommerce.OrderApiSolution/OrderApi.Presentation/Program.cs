using OrderApi.Application.DependencyInjection;
using OrderApi.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseInfrastructurePolicy();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
