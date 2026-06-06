using PROGPOEP2.Services;

var builder = WebApplication.CreateBuilder(args);

// docker
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ApiClientService>(client =>
{
    client.BaseAddress = new Uri("http://glms-backend-api/");
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();