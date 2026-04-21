using Microsoft.EntityFrameworkCore;
using PROGPOEP2.Data;
using PROGPOEP2.Services;

var builder = WebApplication.CreateBuilder(args);
// test change

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LogisticsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<FileService>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();