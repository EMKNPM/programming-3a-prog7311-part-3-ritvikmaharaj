using GLMS.API.Data;
using GLMS.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LogisticsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogisticsDbContext>();

    var retries = 15;

    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Attempting DB migration...");

            db.Database.Migrate();

            Console.WriteLine("DB migration successful");
            break;
        }
        catch (Exception ex)
        {
            retries--;

            Console.WriteLine($"Migration failed. Retries left: {retries}");
            Console.WriteLine(ex.Message);

            Thread.Sleep(3000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();


public partial class Program { }  // unit tests