using CredexApp.BusinessLogic;
using CredexApp.DataLayer;
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

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    var importService = new CsvImportService(context); // Run import service

    string path = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles", "Financial_Statements.csv"); // Path to CSV file

    importService.ImportData(path); // Import data from CSV to dba
}

app.Run();
