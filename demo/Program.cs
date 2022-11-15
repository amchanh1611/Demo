using demo.Models;
using Demo.AppSettings;
using Demo.BUS.BUS;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.AutoMapperProfiles;
using Demo.Helper.JWT;
using Demo.Repository.IRepository;
using Demo.Repository.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

builder.Services.AddDbContextPool<DemoDbContext>(option =>
    option.UseMySql("server=localhost;user id=root;password='';port=3306;database=demo;",
    ServerVersion.AutoDetect("server=localhost;user id=root;password='';port=3306;database=demo;")));

//AutoMapper
builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserBUS, UserBUS>();
builder.Services.AddTransient<IJwtUtils, JwtUtils>();

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining(typeof(UserValidator));

//Add JwtBearer
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://minhchanh.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Secret"])),
            ValidateIssuer = true,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Use JwtBearer
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();