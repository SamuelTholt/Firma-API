using Firma_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CompanyApiDbContext") ?? throw new InvalidOperationException("Connection string 'CompanyApiDbContext' not found.");

builder.Services.AddDbContext<CompanyApiDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(opts =>
{
    opts.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Firma-API",
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.MapScalarApiReference(opts =>
{
    opts.Title = "Firma-API";
    opts.Theme = ScalarTheme.Purple;
    opts.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CompanyApiDbContext>();
    db.Database.Migrate();
}

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"DB: {builder.Configuration.GetConnectionString("CompanyApiDbContext")}");

app.Run();