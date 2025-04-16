using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookingSite.Data;
using BookingSite.Domains.Context;
using BookingSite.Domains.Models;
using BookingSite.Repositories;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services;
using BookingSite.Services.Interfaces;
using BookingSite.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTransient<IDAO<Flight, int>, FlightDAO>();
builder.Services.AddTransient<IService<Flight, int>, FlightService>();


builder.Services.AddTransient<IEmailSender, EmailSender>();
 

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API Employee",
        Version = "version 1",
        Description = "An API to perform Employee operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "CDW",
            Email = "christophe.dewaele@vives.be",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "Employee API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FlightsOverview}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();