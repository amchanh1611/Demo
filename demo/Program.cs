using demo.Models;
using Demo.BUS.BUS;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.AutoMapperProfiles;
using Demo.Helper.JWT;
using Demo.Repository.IRepository;
using Demo.Repository.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
//AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();