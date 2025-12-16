using AuthenticationApi.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
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
