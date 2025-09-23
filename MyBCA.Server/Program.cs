using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using MyBCA.Server.Services.Bus;
using MyBCA.Server.Services.Links;
using MyBCA.Server.Services.Nutrislice;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

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

builder.Services.Configure<MyBCA.Server.Services.Links.LinkOptions>(
    builder.Configuration.GetSection("QuickLinks")
);
builder.Services.AddSingleton<ILinkService, LinkService>();

builder.Services.UseHttpClientMetrics();
builder.Services.AddMetricServer(options =>
{
    options.Port = builder.Configuration.GetValue<ushort>("Metrics:Port");
});

var app = builder.Build();

var rewriteOptions = new RewriteOptions()
    .AddRedirect("^h$", "NewTab", (int)HttpStatusCode.MovedPermanently)
    .AddRedirect("^busapp$", "Bus/List", (int)HttpStatusCode.MovedPermanently)
    .AddRedirect("^busapp/$", "Bus/List", (int)HttpStatusCode.MovedPermanently);
app.UseRewriter(rewriteOptions);


app.UseExceptionHandler("/error");
app.Map("/error", async httpContext =>
{
    var problemDetails = new ProblemDetails
    {
        Type = "/errors/UnknownError",
        Title = "An unexpected error occurred.",
        Status = (int)HttpStatusCode.InternalServerError,
        Detail = "Something went wrong, please try again later.",
        Instance = httpContext.Request.Path
    };

    httpContext.Response.ContentType = "application/json";
    httpContext.Response.StatusCode = problemDetails.Status.Value;
    await httpContext.Response.WriteAsJsonAsync(problemDetails);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseHttpMetrics();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllers();

app.UseCors();

app.Run();
