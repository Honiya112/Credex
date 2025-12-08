using CredexApp.BusinessLogic;
using CredexApp.DataLayer;
using CredexApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Find connection string, otherwise throw error 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register dba + use sqlite + allows AppDbContext to access dba 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// Show detailed errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register CsvImportService for dependency injection
builder.Services.AddScoped<CsvImportService>();

// need to inject FinancialService
builder.Services.AddScoped<FinancialService>();

// Register Transaction
builder.Services.AddScoped<TransactionService>();

// Ana - moved this BEFORE var app = builder.Build().
// seed user into database
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";  // Points to LoginController
    options.LogoutPath = "/Login/Logout";  
    options.AccessDeniedPath = "/Login/AccessDenied";  
});

builder.Services.AddScoped<SignInManager<User>>();


var app = builder.Build();

// MIGRATE FIRST - This creates the tables!

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

/*
app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Login}/{id?}",
    defaults: new { controller = "Login" }
); */

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FinancialHistory}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    // Ana - seed admin first
    await UserService.SeedAsync(services);
    
    var importService = new CsvImportService(context); // Run import service

    string path = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles", "Financial_Statements.csv"); // Path to CSV file

    importService.ImportData(path); // Import data from CSV to dba
}

app.Run();
