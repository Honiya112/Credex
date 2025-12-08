using CredexApp.BusinessLogic;
using CredexApp.DataLayer;
using CredexApp.Models;
using Microsoft.EntityFrameworkCore;

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

// Register Transaction
builder.Services.AddScoped<TransactionService>();

// Ana - moved this BEFORE var app = builder.Build().
// seed user into database
builder.Services.AddDefaultIdentity<User>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    var importService = new CsvImportService(context); // Run import service

    string path = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles", "Financial_Statements.csv"); // Path to CSV file

    importService.ImportData(path); // Import data from CSV to dba
}

app.Run();
