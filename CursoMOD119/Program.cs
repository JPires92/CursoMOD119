using CursoMOD119;
using CursoMOD119.Data;
using CursoMOD119.Data.Seed;
using CursoMOD119.Lib;
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

//Login
//IdentityUser: definição por omissão do ASP.net
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        //Confirm signin account
        options.SignIn.RequireConfirmedAccount = true;

        //Password
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 4;
    })
    .AddRoles<IdentityRole>() //Roles
    .AddEntityFrameworkStores<ApplicationDbContext>(); //onde guarda informações

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//Apply policies a group of users
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.APP_POLICY, policy => policy.RequireRole(AppConstants.APP_POLICY_ROLES));
    options.AddPolicy(AppConstants.APP_ADMIN_POLICY, policy => policy.RequireRole(AppConstants.AAPP_ADMIN_POLICY_ROLES));
});

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
    })
    .AddNToastNotifyToastr(new NToastNotify.ToastrOptions
    {
        ProgressBar = true,
        TimeOut = 5000
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

//Login
app.UseAuthorization();

//Forçar a utilizar localização definida por nós
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);

app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Create one user admin and other operative with roles
Seed();

app.Run();

//Fill a default user admin and operative
void Seed()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;


    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        SeedDatabase.Seed(dbContext, userManager, roleManager);
    }
    catch (Exception ex)
    {

    }
}