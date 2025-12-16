using OrderApi.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureService(builder.Configuration);

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
