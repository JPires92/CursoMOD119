using CursoMOD119;
using CursoMOD119.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


const string defaultCulture = "pt";

var ptCI = new CultureInfo(defaultCulture);
//ptCI.NumberFormat.NumberDecimalSeparator = ".";
//ptCI.NumberFormat.NumberGroupSeparator = " ";
//ptCI.NumberFormat.CurrencyDecimalSeparator = ".";
//ptCI.NumberFormat.CurrencyGroupSeparator = " ";

var supportedCultures = new[]
{
    ptCI,
    new CultureInfo("en"),
    new CultureInfo("fr"),
};

//Forçar a utilizar a mesma formatação em todas as culturas
//foreach(var c in supportedCultures)
//{
//    c.NumberFormat.NumberDecimalSeparator = ".";
//    c.NumberFormat.NumberGroupSeparator = " ";
//    c.NumberFormat.CurrencyDecimalSeparator = ".";
//    c.NumberFormat.CurrencyGroupSeparator = " ";
//}

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services
    .AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Forçar a utilizar localização definida por nós
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
