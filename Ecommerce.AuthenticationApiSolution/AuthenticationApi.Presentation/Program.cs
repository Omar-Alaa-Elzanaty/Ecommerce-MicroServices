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

app.UseHttpsRedirection();
app.UseRouting();
//app.UseInfrastructurePolicy();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
