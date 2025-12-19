using ApiGateway.Presentation.Middleware;
using eCommerce.SharedLibrary.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddCacheManager(x =>
{
    x.WithDictionaryHandle();
});
JWTAuthenticationScheme.AddAuthenticationScheme(builder.Services, builder.Configuration);
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<AttachSignatureToRequest>();
await app.UseOcelot();

app.Run();
