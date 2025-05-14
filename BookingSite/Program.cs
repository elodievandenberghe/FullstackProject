using System.Globalization;
using BookingSite.Controllers;
using Microsoft.EntityFrameworkCore;
using BookingSite.Data;
using BookingSite.Domains.DatabaseContext;
using BookingSite.Domains.Models;
using BookingSite.Repositories;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.Utils;
using BookingSite.ViewModels;
using BookingSite.ViewModels.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(
    options => options.ResourcesPath = "Resources"
);
var supportedCultures = new[] { "nl", "en", "fr" };

builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)  
        .AddSupportedUICultures(supportedCultures); 
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddRazorPages(); 
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BookingSiteContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<BookingSiteContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IDAO<AspNetUser, string>, AspNetUserDao>();
builder.Services.AddTransient<IService<AspNetUser, string>, AspNetUserService>();

builder.Services.AddTransient<IDAO<Airport, int>, AirportDAO>();
builder.Services.AddTransient<IService<Airport, int>, AirportService>();

builder.Services.AddTransient<IRouteDAO, RouteDAO>();
builder.Services.AddTransient<IRouteService, RouteService>();

builder.Services.AddTransient<IMealChoiceDAO, MealChoicesDAO>();
builder.Services.AddTransient<IMealChoiceService, MealchoicesService>();

builder.Services.AddTransient<IDAO<Flight, int>, FlightDAO>();
builder.Services.AddTransient<IService<Flight, int>, FlightService>();

builder.Services.AddTransient<IDAO<Flight, int>, FlightDAO>();
builder.Services.AddTransient<IService<Flight, int>, FlightService>();

builder.Services.AddTransient<ITicketDAO, TicketDAO>();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.AddTransient<IPlaneDAO, PlaneDAO>();
builder.Services.AddTransient<IPlaneService, PlaneService>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddTransient<IBookingDAO, BookingDAO>();
builder.Services.AddTransient<IBookingService, BookingService>();

builder.Services.Configure<TripAdvisorApiKey>(builder.Configuration.GetSection("TripAdvisorApiKey"));
builder.Services.Configure<BrevoApiConfig>(builder.Configuration.GetSection("BrevoApiConfig"));

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "version 1",
        Description = "Our api",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "EV",
            Email = "elodie.vandenberghe@outlook.com",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "GPL",
            Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html"),
        }
    });
});
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.Session";
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
var swaggerOptions = new BookingSite.Options.SwaggerOptions();
builder.Configuration.GetSection(nameof(BookingSite.Options.SwaggerOptions)).Bind(swaggerOptions);
app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});
app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FlightsOverview}/{action=Index}/{id?}");
app.MapRazorPages();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.Run();