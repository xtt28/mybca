using Microsoft.Extensions.Options;
using MyBCA.Services.Bus;
using MyBCA.Services.Links;
using MyBCA.Services.Nutrislice;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.Configure<NutrisliceOptions>(
    builder.Configuration.GetSection("NutrisliceApi")
);
builder.Services.AddHttpClient<INutrisliceService, NutrisliceService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<NutrisliceOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.Configure<BusOptions>(
    builder.Configuration.GetSection("BusSheet")
);
builder.Services.AddHttpClient<IBusService, BusService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<BusOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.Configure<MyBCA.Services.Links.LinkOptions>(
    builder.Configuration.GetSection("QuickLinks")
);
builder.Services.AddSingleton<ILinkService, LinkService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllers();

app.Run();
