
using Bp_back.Context;
using Bp_back.Repositories;
using Bp_back.Services;
using Bp_back.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                            builder =>
                            {
                                builder.WithOrigins(
                            "http://localhost:5000",
                            "https://localhost:44369",
                            "https://localhost:5001").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                            });
});

var connection = Environment.MachineName.ToLower() == "plnw0195" ? new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("WorkConnection") : new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("HomeConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));




builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHubRepository, HubRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IHttpService, HttpService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = TokenHelper.Issuer,
            ValidAudience = TokenHelper.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret))
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();






app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
});
app.UseAuthorization();

app.UseEndpoints(enpoints =>
{
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;




app.Run();
