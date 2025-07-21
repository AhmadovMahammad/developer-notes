using Microsoft.EntityFrameworkCore;
using PortfolioApp;
using PortfolioApp.Data;
using PortfolioApp.Lifecycle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();   // Required for _Host.cshtml
builder.Services.AddServerSideBlazor();    // Enables Blazor Server

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies(); // Enable Lazy Loading in DI
}, ServiceLifetime.Scoped);

// http client registering
//builder.Services.AddHttpClient();

builder.Services.AddHttpClient("OpenWeather", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

    string apiKey = builder.Configuration.GetSection("OpenWeather")["ApiKey"] ?? string.Empty;
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("apiKey", apiKey);
});

builder.Services.AddHttpClient("JsonPlaceholder", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<CounterStateService>();
builder.Services.AddSingleton<ModalStateService>();
builder.Services.AddScoped<UserService>();

// Register services with different lifetimes
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddSingleton<ISingletonService, SingletonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
