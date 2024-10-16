using ArtPatio.Repositories;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the UserProfileRepository
builder.Services.AddScoped<UserProfileRepository>();

builder.Services.AddScoped<ArtworkRepository>();

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<TransactionRepository>();


// Add session services
builder.Services.AddDistributedMemoryCache(); // Adds a memory cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Makes the cookie inaccessible to client-side scripts
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware before authorization
app.UseSession(); // Enable session before the endpoint routing
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
